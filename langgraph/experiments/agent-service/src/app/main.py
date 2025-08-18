import os
from dotenv import load_dotenv
from langchain_google_genai import ChatGoogleGenerativeAI
from langchain_core.prompts import ChatPromptTemplate
from langchain_core.messages import HumanMessage

def test_gemini_chat():
    """
    Loads the Google API key and tests a simple chat with the Gemini model.
    """
    # .env 파일에서 API 키를 로드합니다.
    load_dotenv()
    
    # GOOGLE_API_KEY가 있는지 확인합니다.
    if not os.getenv("GOOGLE_API_KEY"):
        print("❌ Error: GOOGLE_API_KEY not found in .env file.")
        return

    print("✨ Initializing Gemini model...")
    
    # 1. Gemini 모델을 준비합니다. (gemini-1.5-flash는 빠르고 저렴한 최신 모델입니다)
    try:
        model = ChatGoogleGenerativeAI(model="gemini-1.5-flash")
    except Exception as e:
        print(f"❌ Error initializing model: {e}")
        return
        
    print("🚀 Sending a test message to Gemini...")
    
    # 2. 모델에 메시지를 보내고 응답을 받습니다.
    try:
        # LangChain의 기본 메시지 형식인 HumanMessage를 사용합니다.
        message = HumanMessage(
            content="Hello Gemini! In one sentence, tell me why Python is popular."
        )
        result = model.invoke([message])
        
        print("\n--- Gemini's Response ---")
        print(result.content)
        print("------------------------")
        print("\n✅ Success! Your environment is correctly set up to use Gemini.")
    
    except Exception as e:
        print(f"\n❌ An error occurred during invocation: {e}")
        print("Please check if your GOOGLE_API_KEY is correct and has permissions.")

if __name__ == "__main__":
    test_gemini_chat()