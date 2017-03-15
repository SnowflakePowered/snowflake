import { Service } from "./Remoting";
export interface Platform {
    PlatformID: string;
    Metadata: {
        [key: string]: string;
    };
    MaximumInputs: number;
    FriendlyName: string;
    FileTypes: {
        [key: string]: string;
    };
}
export declare class Stone extends Service {
    constructor(rootUrl: string);
    getPlatforms: () => Promise<Map<string, Platform>>;
}
