import "whatwg-fetch";
export interface Response<T> {
    Response: T;
    Error: Error;
}
export interface Error {
    Message?: string;
}
export declare class Service {
    protected rootUrl: string;
    protected serviceName: string;
    protected constructor(rootUrl: string, serviceName: string);
    protected getServiceUrl: (...path: string[]) => string;
}
export declare type Verb = "Create" | "Read" | "Delete" | "Update";
export declare const request: <T>(url: string, payload?: any, verb?: Verb) => Promise<Response<T>>;
