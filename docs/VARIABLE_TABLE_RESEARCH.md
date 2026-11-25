# 单PLC空压站学习点表研究依据

## 研究目的

本说明为“一个 Modbus 从站代表整个空压站 PLC”的学习用变量表提供依据。研究只决定应当包含哪些变量类别和控制语义，不复制任何厂商的私有寄存器地址。

拟定点表是本项目自己的 PLC/模拟器内存映射，适用于教学、仿真和上位机联调。真实设备投用前，必须按照实际空压机、变频器、仪表和 PLC 程序重新确认地址、单位、量程、字节序、权限与安全条件。

## Modbus 数据区与地址

Modbus 官方规范定义四类主要数据对象：只读离散输入、可读写线圈、只读输入寄存器、可读写保持寄存器。规范同时说明 PDU 地址从 0 开始，文档中的第 1 个对象在请求中通常使用地址 0。[Modbus Application Protocol Specification V1.1b3](https://www.modbus.org/docs/Modbus_Application_Protocol_V1_1b3.pdf)

因此学习点表采用：

- Discrete Inputs：PLC反馈、运行状态和报警位。
- Coils：启停、复位、自动模式和阀门命令。
- Input Registers：压力、温度、电流、流量、压差、运行时间和故障代码。
- Holding Registers：加载/卸载压力、报警阈值和仿真工况设定。

点表同时记录显示地址与 0 基协议偏移，避免把 `40001` 或 `30001` 直接当作 NModbus 请求地址。

## 空压机变量类别

Atlas Copco 的水冷螺杆空压机技术规格要求控制器支持全载/空载与启停调节，使用排气压力维持可调压力范围，并支持本地、远程或网络控制。规格还列出排气压力、主机出口温度、环境温度、空压机状态、电机过载、运行小时、加载小时和调节器小时等监控量；保护信息包括急停、出口温度、驱动/冷却风机过载和维护警告。[Atlas Copco GA37+ compressor specification](https://www.atlascopco.com/content/dam/atlas-copco/local-countries/united-kingdom/documents/20-40kw-25-50-hp/GA37plus-37kW-8.5bar-Water-Cooled.pdf)

Atlas Copco 对加载/卸载控制的说明表明，系统压力和上下限设定共同决定加载、卸载或停机；进气阀是加载/卸载的关键执行部件，控制系统还需要考虑低用气量时的空载运行与电机启动次数限制。[Atlas Copco load/unload/stop control](https://www.atlascopco.com/en-us/compressors/wiki/compressed-air-articles/load-unload-stop-control)

据此，学习闭环保留这些最小空压机变量：

- 远程允许、就绪、运行、加载、卸载、故障和急停反馈。
- 启动、停止和故障复位命令。
- 排气/总管压力、出口温度、电机电流、产气流量、状态代码、故障代码和累计运行时间。
- 加载压力、卸载压力和高温报警阈值。

急停和设备保护只作为只读反馈。安全联锁继续由 PLC/本地控制器执行，上位机不是唯一安全层。

## 泵和驱动变量类别

台达 VFD-EL-W 官方手册展示了 RS-485 可作为频率命令来源，并区分频率命令来源与运行命令来源。手册也提供运行、频率到达、故障、警告、驱动就绪、多泵系统错误等状态，以及累计电机运行时间和故障记录。[Delta VFD-EL-W user manual](https://filecenter.deltaww.com/Products/download/06/060101/Manual/DELTA_IA-MDS_VFD-EL-W_UM_EN_20240904.pdf)

Schneider Altivar 21 官方 Modbus 手册同样区分命令字、频率设定、当前状态、运行/停止、急停、本地/远程、报警和故障代码，并明确通信设定值具有单位和有效范围。[Schneider Electric Altivar 21 Modbus manual](https://download.schneider-electric.com/files?p_Doc_Ref=1769687-EN&p_File_Ext=.PDF)

据此，学习点表中的冷却泵只保留：

- 启动、停止和自动模式命令。
- 运行、故障和远程允许反馈。
- 电流或频率中的必要监测量。

第一阶段不复制具体变频器寄存器，也不把泵的所有厂家参数暴露给上位机。PLC负责把现场驱动状态映射到统一点表。

## 储气罐、过滤器与系统变量

储气罐和过滤器通常是被测对象，而不是独立 Modbus 从站。学习模型需要它们，是因为它们把“产气、用气和压力”连接成完整闭环：

- 储气罐/总管压力反映供气量与车间用气量之间的差值。
- 过滤器压差用于演示堵塞报警。
- 车间用气量是仿真扰动输入。
- 冷却水温度和液位用于演示辅机运行条件与报警。

这些地址属于本项目自定义的 PLC 汇总映射，不来自某个品牌过滤器或储气罐的通用寄存器表。

## 点表规模原则

第一阶段应满足闭环学习，但避免复制完整工厂点表：

- 1 个 PLC 从站，默认从站地址 1。
- 1 台空压机。
- 1 个储气罐/公共总管压力模型。
- 1 台冷却泵和必要冷却条件。
- 1 个过滤器压差。
- 车间用气量作为仿真输入。
- 完整的启停、加载/卸载、高温报警、故障复位和通信读写链路。

不在第一阶段加入四机调度、复杂维护参数、能源KPI、多个干燥机和全部辅机联锁。这些内容留给后续阶段。

## 不能从市场资料直接得到的内容

下列内容必须由本项目定义，或在真实现场由厂家/PLC工程师确认：

- Modbus 从站地址和串口参数。
- 每个变量的最终寄存器偏移。
- 32 位数据的寄存器字序。
- 现场传感器量程和缩放。
- 控制命令是脉冲、保持还是命令字。
- 写入权限、联锁条件和写后回读规则。
- 故障代码与具体机型的对应关系。

Atlas Copco 官方也明确表示不存在覆盖所有机型的通用 Elektronikon 故障代码表，准确诊断必须使用具体机型说明书。[Atlas Copco Elektronikon faults and alarms](https://www.atlascopco.com/en-nz/compressors/air-compressor-blog/elektronikon-alarms-and-fault-codes)

## 结论

可以基于这些官方资料设计一份基本完整且适合学习的闭环变量表。该表应被标记为“项目自定义仿真/PLC映射”，而不是“市场通用地址表”。它能够支撑 NModbus 主站、Modbus Slave 手工验证、独立代码模拟器、WinForms 实时显示、控制、报警和趋势练习；不能直接替代真实设备协议表。
