from fastapi import FastAPI
from app.api.llm import query

# FastAPI 앱 초기화
app = FastAPI(
    title="LLM Query API",
    description="LangChain을 사용하여 LLM에 쿼리하는 구조화된 테스트용 API입니다.",
    version="1.0.0",
)

# query 라우터를 앱에 포함시킵니다.
# /api/v1/query 경로로 접근할 수 있게 됩니다.
app.include_router(query.router, prefix="/api/v1/query", tags=["Query"])

@app.get("/")
def read_root():
    """서버 상태 확인을 위한 기본 GET 엔드포인트"""
    return {"status": "LLM Query API is running"}