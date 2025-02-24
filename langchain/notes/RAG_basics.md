# RAG (Retrieval-Augmented Generation)

## 개념
**RAG**는 LLM(Large Language Models)의 성능을 향상시키기 위해 외부 데이터를 활용하여 더욱 정확하고 유용한 응답을 생성하는 기술입니다. LLM 단독으로는 최신 정보 부족, 환각현상(hallucination) 등 여러 한계가 있으므로, RAG는 검색과 생성 과정을 결합하여 이를 해결합니다.

## 동작 원리
1. **사용자 쿼리 입력**: 사용자가 질문이나 요청을 입력합니다.
2. **정보 검색**: 외부 데이터베이스(DB), 웹(Web), 문서 저장소 등에서 관련 데이터를 검색합니다.
3. **쿼리와 정보 결합**: 검색된 데이터를 사용자 입력과 함께 모델의 입력으로 제공합니다.
4. **응답 생성**: 강화된 입력을 기반으로 LLM이 응답을 생성합니다.

## 구성 요소
1. **Retrieval (검색)**
   - 외부 지식 소스에서 관련 데이터를 검색.
   - 예: 검색 엔진, 벡터 데이터베이스, 문서 저장소 등.

2. **Augmentation (강화)**
   - 검색된 데이터를 LLM의 입력으로 통합하여 컨텍스트를 강화.
   - 통합 방식: 검색 데이터를 프롬프트에 포함하거나, 모델에 추가적인 피처(feature)로 제공.

3. **Generation (생성)**
   - LLM이 검색된 데이터를 활용하여 적합한 응답 생성.

## 장점
1. **최신 정보 제공**: 외부 데이터 검색을 통해 LLM 단독으로 제공할 수 없는 최신 정보를 반영.
2. **환각현상 방지**: LLM이 잘못된 정보를 생성하는 것을 줄이거나 방지.
3. **도메인 특화 가능**: 특정 산업(법률, 의료 등)에 적합한 지식을 응답에 반영.
4. **모델 업데이트 필요 없음**: 모델 자체를 재학습하지 않고도 지식 확장 가능.

## 주요 사용 사례
- **고객 지원**: 고객의 질문에 대한 최신 정보 기반의 정확한 응답 제공.
- **전문가 시스템**: 법률, 의료 등 특정 도메인에서의 전문 지식 활용.
- **문서 기반 질의응답**: 대량의 문서에서 정보를 검색하고 요약하여 응답 생성.

## 구현 시 고려 사항
1. **검색 정확도**: 검색 시스템이 관련성 높은 정보를 반환해야 응답 품질이 보장됨.
2. **응답 속도**: 검색과 생성 단계가 추가되므로, 효율적인 캐싱 및 최적화가 필요.
3. **데이터 신뢰성**: 신뢰할 수 있는 데이터 소스에서 정보를 가져와야 함.
4. **프롬프트 설계**: 검색된 데이터를 적절히 통합하는 프롬프트 설계가 중요.

## 기술 스택
- **LLM**: OpenAI GPT, Google Bard, Hugging Face Transformers 등.
- **벡터 데이터베이스**: Pinecone, Weaviate, Milvus 등.
- **프레임워크**: LangChain, Haystack.

