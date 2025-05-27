# Semantic Kernel Agent Framework

Semantic Kernel Agent Framework는 Microsoft가 개발한 Semantic Kernel의 확장 기능으로, 대형 언어 모델(LLM)을 활용한 에이전트 기반 애플리케이션을 효과적으로 구축하기 위한 구성 요소들을 제공합니다. 이 프레임워크는 LLM, 도구, 메모리, 플러그인 등을 통합하여 보다 구조화된 에이전트 시스템을 만들 수 있게 해줍니다.

## 주요 개념

### 1. Agent

- 고유한 목표(goal), 역할(role), 상태(state)를 갖는 실행 단위
- 내부적으로 하나 이상의 스킬(skill)을 호출하거나, 다른 Agent와 상호작용할 수 있음

### 2. Kernel

- Semantic Kernel의 중심 객체로, 에이전트가 사용하는 LLM, 메모리, 스킬, 플러그인 등을 포함

### 3. Skill / Plugin

- 재사용 가능한 기능 단위 (예: 요약, 번역, 계산 등)
- C# 메서드, 프롬프트 등으로 정의 가능하며, 에이전트가 호출하여 사용

### 4. Planner

- 주어진 목표를 분석하여 필요한 스킬 실행 계획을 자동으로 생성
- 단일 목표를 여러 단계로 분해하여 순차적으로 실행할 수 있음

### 5. Memory

- 에이전트 간 또는 세션 간 상태를 저장하고 검색할 수 있는 구성 요소
- 임베딩 기반 검색 메모리나 간단한 키-값 저장소 형태로 사용 가능

## MCP (Model Context Protocol)

MCP는 Semantic Kernel Agent Framework에서 도입한 메시징 프로토콜로, 에이전트, 사용자, 도구 간 상호작용을 표준화된 JSON 메시지 형태로 처리합니다.

### MCPMessage의 특징

- 역할(role), 콘텐츠(content), 시간(timestamp), 출처(source) 등 포함
- LLM의 function-calling 또는 tool-calling 기능과 직접 연결 가능

## 특징 및 장점

- .NET 기반으로 강력한 확장성과 통합성 제공
- OpenAI, Azure OpenAI, HuggingFace 등 다양한 모델 연동 가능
- 플러그인 기반으로 기능을 분리하여 재사용 가능
- MCP를 통한 에이전트 간 메시지 교환 표준화

## 사용 예시

- 문서 요약, 질의응답, 자동화된 분석 흐름 구성
- 다중 에이전트 기반 협업 시스템 (예: 작성 → 검토 → 수정)
- 사용자 요청에 따라 동적으로 플래너가 스킬 시퀀스를 생성하고 실행

## 참고 링크

- [Semantic Kernel GitHub](https://github.com/microsoft/semantic-kernel)

LangChain, LangGraph와는 달리 .NET 생태계에 밀접하게 맞춰져 있으며, 강력한 구조화, 통합형 LLM 활용 방식을 지원합니다.
