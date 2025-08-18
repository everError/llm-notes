from langchain_openai import ChatOpenAI
from langchain_core.prompts import ChatPromptTemplate
from ..core import config  # config.py에서 API 키를 사용하기 위함 (사용은 안 하지만 연결 구조 예시)

# LangChain 구성요소를 설정합니다.
try:
    model = ChatOpenAI(model="gpt-4o")
    prompt = ChatPromptTemplate.from_template("Tell me a short joke about {topic}")
    chain = prompt | model
except Exception as e:
    chain = None
    print(f"Warning: Could not initialize LangChain chain. Error: {e}")

async def get_ai_joke(topic: str) -> str:
    """LangChain 체인을 실행하여 AI의 응답을 받아옵니다."""
    if chain is None:
        raise ValueError("AI model chain is not available.")
    
    result = await chain.ainvoke({"topic": topic})
    return result.content