.PHONY: help build up down restart logs clean setup

help: ## Show this help message
	@echo 'Usage: make [target]'
	@echo ''
	@echo 'Available targets:'
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  %-15s %s\n", $$1, $$2}' $(MAKEFILE_LIST)

setup: ## Initial setup - copy config files
	@echo "Setting up configuration files..."
	@if [ ! -f yuuka-chan/config.json ]; then \
		cp yuuka-chan/config.docker.json yuuka-chan/config.json; \
		echo "Created yuuka-chan/config.json from template"; \
		echo "⚠️  Please edit yuuka-chan/config.json with your Discord bot token"; \
	else \
		echo "config.json already exists"; \
	fi
	@if [ ! -f .env ]; then \
		cp .env.example .env; \
		echo "Created .env from template"; \
	fi

build: ## Build all Docker images
	docker compose build

up: ## Start all services
	docker compose up -d

up-dev: ## Start all services in development mode (with logs)
	docker compose up

down: ## Stop all services
	docker compose down

restart: ## Restart all services
	docker compose restart

logs: ## Show logs from all services
	docker compose logs -f

logs-api: ## Show logs from wallet API
	docker compose logs -f wallet-api

logs-bot: ## Show logs from Discord bot
	docker compose logs -f yuuka-bot

clean: ## Remove containers, networks, and volumes
	docker compose down -v
	docker system prune -f

rebuild: ## Rebuild and restart all services
	docker compose up -d --build

status: ## Show status of all services
	docker compose ps

shell-api: ## Open shell in wallet-api container
	docker compose exec wallet-api /bin/bash

shell-bot: ## Open shell in yuuka-bot container
	docker compose exec yuuka-bot /bin/bash

prod-up: ## Start services in production mode
	docker compose -f docker-compose.yaml -f docker-compose.prod.yaml up -d

prod-build: ## Build services for production
	docker compose -f docker-compose.yaml -f docker-compose.prod.yaml build

prod-down: ## Stop production services
	docker compose -f docker-compose.yaml -f docker-compose.prod.yaml down
