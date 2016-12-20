import "isomorphic-fetch";

export interface Response<T> {
    Response : T
    Error : Error
}

export interface Error {
    Message?: string
}

export async function request<T>(url:string, payload:string = "") : Promise<Response<T>>  {
    let result = await fetch(url);
    if (result.ok) {
        let json = await result.json()
        return { Response: json.Response, Error:json.Error }
    }
    return { Response: null, Error: { Message: "Unable to resolve promise." }}
}