from pydantic import BaseModel

class QueryRequest(BaseModel):
    """API 요청 시 받을 데이터 모델"""
    topic: str

class QueryResponse(BaseModel):
    """API 응답 시 보낼 데이터 모델"""
    response: str