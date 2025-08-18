from dotenv import load_dotenv
import os

# .env 파일에서 환경 변수를 로드합니다.
load_dotenv()

# 환경 변수에서 API 키를 가져옵니다.
OPENAI_API_KEY = os.getenv("OPENAI_API_KEY")

# 만약 API 키가 없다면 에러를 발생시킬 수 있습니다.
if OPENAI_API_KEY is None:
    print("Warning: OPENAI_API_KEY is not set in the .env file.")