# 基于 .NET Aspire 和 Blazor 的问卷系统

本人毕设

- 技术架构：ASP.NET Core、.NET Aspire、Blazor、PostgreSQL、Redis、RabbitMQ、Docker
- 项目概述：设计集结问卷创建、分发、数据处理及数据分析功能为一体的问卷系统
- 核心技术：
  - 架构设计：基于 .NET Aspire 构建 6 个微服务（认证/问卷/邮箱/数据处理等），服务间通信采用 HTTP + 消息队列
  - 认证体系：实现 JWT + Refresh Token 双令牌机制，集成 ASP.NET Identity 与Argon2 算法
  - 高并发处理：采用 RabbitMQ 队列削峰，结合 Redis 缓存热点数据
  - 可视化方案：Blazor 前端集成 MudBlazor，实现多种实时分析图表
