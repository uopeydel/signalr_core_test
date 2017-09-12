import { ConnectionClosed } from "./Common";
import { IConnection } from "./IConnection";
import { Observable } from "./Observable";
import { IHubConnectionOptions } from "./IHubConnectionOptions";
export { TransportType } from "./Transports";
export { HttpConnection } from "./HttpConnection";
export { JsonHubProtocol } from "./JsonHubProtocol";
export { LogLevel, ILogger } from "./ILogger";
export { ConsoleLogger, NullLogger } from "./Loggers";
export declare class HubConnection {
    private readonly connection;
    private readonly logger;
    private protocol;
    private callbacks;
    private methods;
    private id;
    private connectionClosedCallback;
    constructor(urlOrConnection: string | IConnection, options?: IHubConnectionOptions);
    private onDataReceived(data);
    private invokeClientMethod(invocationMessage);
    private onConnectionClosed(error);
    start(): Promise<void>;
    stop(): void;
    stream<T>(methodName: string, ...args: any[]): Observable<T>;
    send(methodName: string, ...args: any[]): Promise<void>;
    invoke(methodName: string, ...args: any[]): Promise<any>;
    on(methodName: string, method: (...args: any[]) => void): void;
    onClosed: ConnectionClosed;
    private createInvocation(methodName, args, nonblocking);
}
