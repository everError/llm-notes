# from langchain_google_genai import ChatGoogleGenerativeAI
from langchain_openai import ChatOpenAI
from app.core.config import settings

# google
# llm = ChatGoogleGenerativeAI(
#     model="gemini-pro", 
#     google_api_key=settings.GOOGLE_API_KEY
# )

# OpenAI
llm = ChatOpenAI(
    model="gpt-4o-mini",
    openai_api_key=settings.OPENAI_API_KEY
)

def get_llm_response(query: str) -> str:
    """LLM에 쿼리를 보내고 응답 텍스트를 반환합니다."""
    response = llm.invoke(query)
    return response.content