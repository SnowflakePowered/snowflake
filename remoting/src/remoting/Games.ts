import { Response, request } from "./Remoting";

export interface Game {

}

export async function getGames() : Promise<Game[]>  {
   return (await request<Game[]>("http://localhost:9696/games")).Response;
}