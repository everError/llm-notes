from fastapi import APIRouter
from app.schemas.query import QueryRequest, QueryResponse
from app.services import llm_service

router = APIRouter()

@router.post("/", response_model=QueryResponse)
async def handle_query(request: QueryRequest) -> QueryResponse:
    """
    사용자 쿼리를 받아 LLM 서비스에 전달하고 답변을 반환합니다.
    """
    answer = llm_service.get_llm_response(request.query)
    return QueryResponse(answer=answer)