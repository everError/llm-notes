# 🦀 MCP Rust SDK 실습 가이드

> 이 문서는 [`modelcontextprotocol/rust-sdk`](https://github.com/modelcontextprotocol/rust-sdk)를 활용하여 **Rust 기반 MCP (Model Context Protocol)** 서버 및 툴을 직접 실습하기 위한 가이드입니다.

---

## 📌 MCP Rust SDK란?

**MCP Rust SDK**는 [Model Context Protocol](https://github.com/modelcontextprotocol/spec)에 기반한 Rust 개발용 SDK로, 다음과 같은 기능을 제공합니다:

* MCP 메시지 표준 처리
* Tool 정의 및 실행
* Router 구성 (툴 라우팅 서버)
* Interpreter 구성 (자연어 명령 → MCP 메시지 변환)
* Axum 기반 HTTP API 제공

이를 통해 **Claude Desktop**, **OpenDevin** 등과의 통신이 가능합니다.

---

## 📦 설치 및 초기 실행

### 1. 레포지토리 클론 및 빌드

```bash
git clone https://github.com/modelcontextprotocol/rust-sdk.git
cd rust-sdk
cargo build
```

### 2. 예제 실행

```bash
cargo run --example tool_echo
```

---

## 🔧 Tool 예제 구성

### 📁 examples/tool\_echo.rs

```rust
use mcp::{Tool, ToolInput, ToolOutput, run_tool};

struct EchoTool;

impl Tool for EchoTool {
    fn call(&self, input: ToolInput) -> ToolOutput {
        ToolOutput::text(format!("Echo: {}", input.text))
    }
}

fn main() {
    run_tool(EchoTool {});
}
```

### 설명

* `Tool` trait을 구현하여 `call` 메서드 정의
* `run_tool`로 MCP 메시지를 받아 툴로 전달하고 결과 응답

---

## 🌐 ToolRouter 서버 구성

```rust
let mut router = ToolRouter::new();
router.register_tool("echo", EchoTool {});
run_router(router);
```

* MCP 메시지를 수신하고 `tool.name` 기준으로 툴에 라우팅
* `axum`을 통해 HTTP API로 노출됨 (기본 포트: 3000 등)

---

## 📮 MCP 메시지 예시

```json
{
  "request_id": "abc123",
  "tool_call": {
    "name": "echo",
    "arguments": {
      "text": "hello"
    }
  }
}
```

### 응답 예시

```json
{
  "request_id": "abc123",
  "output": {
    "text": "Echo: hello"
  }
}
```

---

## ✅ 실습 순서 추천

1. `cargo run --example tool_echo` 실행해보기
2. `tool_calculator.rs`와 같은 새 툴 작성해보기
3. `ToolRouter` 서버에 여러 툴 등록
4. Postman 또는 클라이언트 코드로 JSON POST 요청 실습
5. 실제 프로젝트에 MCP 인터페이스 붙여보기

---

## 📚 참고

* [modelcontextprotocol/rust-sdk](https://github.com/modelcontextprotocol/rust-sdk)

---

> 실습에 필요한 예제 코드는 차후 `/examples/` 또는 별도 실습용 폴더로 추가해도 좋습니다.
