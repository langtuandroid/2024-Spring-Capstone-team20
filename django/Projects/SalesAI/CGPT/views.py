from django.shortcuts import render
from django.http import JsonResponse
from openai import OpenAI
import json
import os
from django.views.decorators.csrf import csrf_exempt

client = OpenAI(api_key = os.environ.get('OPENAI_API_KEY'),)
item = {}
NPC = {}

# 파일에서 시스템 메시지 읽기
def read_system_message(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        return file.read()
def get_GPTanswer(system, prompt):
    query = client.chat.completions.create(
        model="gpt-4o-mini",
        messages = [
            {"role": "system", "content": system},
            {"role": "user", "content": input}
        ],
        max_tokens=500,
        n=1,
        stop=None,
        temperature=0.5,
        response_format={"type" : "json_object"}
    )
    response = query.choices[0].message.content
    print("AI"+response)
    return response

def make_sale_chat(prompt):
    system_message_content = read_system_message('CGPT/system_message.txt')
    system_message_content += """
        Please respond in the following JSON format:
        {
            "thought": "Your thought about the item.",
            "reason": "Reasons supporting your thought.",
            "emotion": "Your emotional reaction (e.g., neutral, positive, negative).",
            "suggestedPrice": "The price you would suggest.",
            "reaction": "A suitable reaction based on the provided context."
        }
        Ensure that your response is a valid JSON object.
        """
    return get_GPTanswer(system_message_content, prompt)
def make_ready_to_buy(prompt):
    return JsonResponse({'reply': 'Item, NPC well received.'})
def clear_everything(request, isSuccess):
    if 'chat_history' in request.session and request.session['chat_history']:
        system_message_content = "Please provide a concluding response to the conversation so far."
        if (isSuccess):
            system_message_content += "Indicate that the user is willing to buy at the current price in the reaction."
        else:
            system_message_content += "Inform that the user is not interested in buying at the current price and is now leaving."
        system_message_content += """
            Please respond in the following JSON format:
            {
            "reaction":"A suitable reaction based on the provided context."
            }
            Ensure that your response is a valid JSON object.
            """
        response = get_GPTanswer(system_message_content, "")

        # clear history
        request.session['chat_history'] = []
        return response
    else:
        return JsonResponse({'reply': 'No chat history to clear.'})

def update_history(prompt, request):
    # 세션에 'chat_history' 키가 없으면 빈 리스트로 초기화
    if 'chat_history' not in request.session:
        request.session['chat_history'] = []

    # 세션에서 대화 히스토리 가져오기
    chat_history = request.session['chat_history']

    # 현재 사용자의 입력을 대화 히스토리에 추가
    chat_history.append({"role": "user", "content": prompt})

    # 대화 히스토리를 세션에 다시 저장
    request.session['chat_history'] = chat_history

    return chat_history


@csrf_exempt
def query_view(request):
    if request.method == 'POST':
        print("Received POST data:", request.body)  # 수신된 데이터 출력
        try:
            data = json.loads(request.body)
            request_type = data.get('type')
            prompt = data.get('request')

            if request_type == 'Clear':
                return clear_everything(request, True)

            # else
            messages = update_history(prompt, request)
            if isinstance(messages, list):
                str_messages = str(messages)
                response = "No valid request type provided."
                
                if request_type == 'Init':
                    response = make_ready_to_buy(str_messages)

                elif request_type == 'Chat':
                    response = make_sale_chat(str_messages)
                    request.session['chat_history'].append({"role": "assistant", "content": response})

                return JsonResponse({'reply': response})

            #if prompt and prompt.split()[-1].lower() == "clear":
            #    clear_everything(request)

        except json.JSONDecodeError:
            print("에러났다!")
            return JsonResponse({'error': 'Invalid JSON.'}, status=400)

    return render(request, 'index.html')
