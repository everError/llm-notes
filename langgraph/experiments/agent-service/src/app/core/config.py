from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    # .env 파일에서 읽어올 환경 변수들을 정의합니다.
    GOOGLE_API_KEY: str
    OPENAI_API_KEY: str | None = None # 선택적 변수

    # .env 파일을 참조하도록 설정
    model_config = SettingsConfigDict(env_file=".env")

# 설정 객체를 생성하여 다른 파일에서 import하여 사용합니다.
settings = Settings()