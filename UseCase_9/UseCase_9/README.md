# Sentiment Analysis Web API

## Overview
This project is a Web API backend that connects users to OpenAI's GPT models for sentiment analysis. Users interact via a browser or app, sending requests to the backend, which then communicates with OpenAI and returns AI-generated responses.

## Architecture
![alt text](image.png)

**Request Path:**
1. User → Web API → OpenAI

**Response Path:**
2. OpenAI → Web API → User

- The user sends an HTTP request (e.g., to `/api/chat` or `/api/completions`).
- The backend server (Web API) forwards the request to the OpenAI API.
- The OpenAI API processes the request and sends an AI response back to the Web API.
- The Web API returns a JSON response to the user.

## Endpoints
- `/api/chat`: Handles chat-based interactions.
- `/api/completions`: Handles completion requests.

## Technologies
- ASP.NET Core Web API
- Azure OpenAI Service
- C#

## How to Run
1. Clone the repository.
2. Configure your OpenAI API key in `appsettings.json`.
3. Build and run the project using Visual Studio or `dotnet run`.

## Security
- Do not commit secrets (API keys) to the repository.
- Use environment variables or secret managers for sensitive data.

## License
MIT
