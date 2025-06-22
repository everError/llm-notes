# 🧠 MCP 에이전트, 서버, 클라이언트, 툴 아키텍처 관계 정리

이 문서는 특정 SDK나 프레임워크에 종속되지 않은 일반적인 **Model Context Protocol (MCP)** 기반 구조에서, **AI 에이전트**, **MCP 클라이언트**, **MCP 서버**, 그리고 **서버 내 Tool** 간의 역할과 관계를 명확히 정리한 것입니다.

---

## 🧩 주요 구성 요소

### 1. 🧠 에이전트 (Agent)

* LLM을 활용하여 사용자 입력을 분석하고, 필요한 경우 외부 도구(툴)를 호출하는 의사결정 주체
* MCP 메시지를 생성하고 이를 클라이언트를 통해 서버로 전달
* 예: LangGraph, Semantic Kernel, Claude Desktop, OpenDevin, 커스텀 Python/C#/Rust Agent

### 2. 🔌 MCP 클라이언트

* 에이전트 내부에서 동작하며, MCP 메시지를 외부 MCP 서버로 전달하는 중계자
* MCP 메시지(JSON)를 `HTTP`, `stdio`, `WebSocket` 등으로 전송하고 응답을 받아옴
* 종종 MCP 서버를 `subprocess`로 실행하거나 `child_process`로 관리함
* 예: Rust로 작성된 MCP 클라이언트, LangGraph의 MCPStdioTool 래퍼 등

### 3. 🛰️ MCP 서버

* 클라이언트로부터 받은 MCP 메시지를 파싱하고, 적절한 MCP Tool을 찾아 실행한 뒤 응답을 반환
* JSON 기반의 MCP 메시지를 받고, MCP 응답 포맷으로 결과를 돌려줌
* HTTP or stdio 기반으로 구현 가능
* 예: rmcp 기반 서버, SK에서 MCP 서버로 설정된 앱, Node.js MCP 서버

### 4. 🔧 Tool (서버 내부 기능)

* 실제로 사용자의 요청을 처리하는 비즈니스 로직 또는 함수 단위
* 서버 내에 등록되어 있으며, MCP 메시지의 `tool_call.name`으로 라우팅됨
* 예: EchoTool, CalculatorTool, WeatherTool 등

---

## 🔁 일반적인 실행 흐름

```text
사용자 입력
   ↓
[🧠 에이전트 (LLM)]
   ↓ (tool_call 필요 판단)
[MCP 메시지 생성]
   ↓
[🔌 MCP 클라이언트 (Agent 내부)]
   ↓
[MCP 메시지 전송]
   ↓
[🛰️ MCP 서버]
   ↓
[🔧 Tool 실행 (예: echo)]
   ↓
[MCP 응답 반환]
   ↓
[에이전트가 응답 해석 → 최종 답변 생성]
```

---

## 🔄 각 컴포넌트 간 관계 요약

| 구성 요소     | 호출 대상     | 역할 요약                            |
| --------- | --------- | -------------------------------- |
| 에이전트      | MCP 클라이언트 | tool 호출 필요 시 MCP 메시지 생성 및 전송     |
| MCP 클라이언트 | MCP 서버    | 메시지 전송 및 응답 수신 (transport layer) |
| MCP 서버    | Tool      | tool\_call.name에 따라 해당 기능 실행     |
| Tool      | 내부 로직     | 실제 응답 생성, 외부 API, 계산 등 수행        |

---

## 📌 핵심 개념 요약

* MCP는 에이전트가 외부 도구(툴)를 호출할 수 있도록 만든 메시지 기반 통신 프로토콜이다.
* 에이전트는 항상 "클라이언트 역할"을 하며, 서버는 "툴 제공자" 역할을 한다.
* MCP 서버는 여러 언어로 작성된 다양한 툴을 포함할 수 있고, 이 서버 자체도 다양한 실행 환경(HTTP, stdio 등)에서 운용 가능하다.
* 클라이언트는 단순히 메시지를 포워딩하는 중간자이며, 필요하면 MCP 서버를 실행하는 역할까지 맡을 수도 있다.
