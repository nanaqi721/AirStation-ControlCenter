# 空压机集控项目资料包

这是一个用于从 0 到 1 开发空压机集控上位机的项目上下文包，适合第一次做上位机项目时持续使用。

## 文件说明

- [AGENTS.md](../AGENTS.md)：Agent 工作规则、整体架构、功能范围、安全约束和完成标准。
- [PROJECT_STATUS.md](PROJECT_STATUS.md)：项目进度与决策记录；后续每次实现并验证功能后都要更新。
- [REFERENCE_NOTICE.md](REFERENCE_NOTICE.md)：参考资料来源、使用边界、版权和工业安全声明。
- [VIDEO_TITLES_P26-P97.md](VIDEO_TITLES_P26-P97.md)：P26—P97 视频标题对应的功能索引。
- [MODBUS_COMMUNICATION_IMPLEMENTATION.md](MODBUS_COMMUNICATION_IMPLEMENTATION.md)：从 Modbus Slave 手工验证到独立 NModbus 从站的通信实施指南。
- [VIRTUAL_AIR_COMPRESSOR_SIMULATION.md](VIRTUAL_AIR_COMPRESSOR_SIMULATION.md)：最小闭环、四机集控和辅机系统的仿真范围与设备行为指南。
- [空压站PLC学习用Modbus变量表.xlsx](../空压站PLC学习用Modbus变量表.xlsx)：一个从站代表整个空压站 PLC 的第一阶段权威点表，包含变量、枚举、读取分组和闭环验证场景。
- [VARIABLE_TABLE_RESEARCH.md](VARIABLE_TABLE_RESEARCH.md)：变量选择和边界的官方资料研究记录；不把任何厂商地址当作通用地址。
- [reference/课件资料.zip](../reference/课件资料.zip)：用户提供的原始参考资料，按原样保存。

## 建议使用顺序

1. 根目录的 [AGENTS.md](../AGENTS.md) 保存完整协作规则，[README.md](../README.md) 用作项目导航；其他项目文档放在 `docs/`。点表保留在项目根目录，原始资料保留在 [reference/](../reference/)。
2. 每次继续项目时，先读取 [AGENTS.md](../AGENTS.md) 和 [PROJECT_STATUS.md](PROJECT_STATUS.md)。
3. 默认以讨论、解释和对话内示例代码为主；只有用户明确要求写入时才修改项目文件。
4. 每次只实现一个可验证的小功能，并在构建或人工验证通过后更新 [PROJECT_STATUS.md](PROJECT_STATUS.md)。
5. 涉及课件图片、代码、DLL、默认账号或通信参数前，先阅读 [REFERENCE_NOTICE.md](REFERENCE_NOTICE.md)。
6. 涉及串口、NModbus、地址、轮询、读写或重连时，按 [MODBUS_COMMUNICATION_IMPLEMENTATION.md](MODBUS_COMMUNICATION_IMPLEMENTATION.md) 实施。
7. 涉及空压机、储气罐、多机调度、过滤器或冷却系统仿真时，按 [VIRTUAL_AIR_COMPRESSOR_SIMULATION.md](VIRTUAL_AIR_COMPRESSOR_SIMULATION.md) 实施。
8. 配置 Modbus Slave、NModbus 主站或独立模拟器时，以 [空压站PLC学习用Modbus变量表.xlsx](../空压站PLC学习用Modbus变量表.xlsx) 为地址、类型和读写权限的单一事实来源。

## 当前状态

- 技术栈已经确定：WinForms、.NET 7、NModbus、SQLite。
- 已整理总体架构和完整功能范围。
- 已完成第一阶段学习用 Modbus 变量表设计；尚未实施通信代码或从站配置。
- 正式产品代码尚未开始实现。
- 下一步是按变量表配置 Modbus Slave 的最小读取块，验证第一条 NModbus 读取链路。

生成日期：2026-11-25（Asia/Shanghai）
