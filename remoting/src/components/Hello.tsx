import * as React from "react";
import * as Games from "../remoting/Games";

export interface HelloProps { compiler: string; framework: string; }

// 'HelloProps' describes the shape of props.
// State is never set so we use the 'undefined' type.
export class Hello extends React.Component<HelloProps, undefined> {
    render() {
        return <h1>Everything is working! {this.props.compiler} and {this.props.framework}!</h1>;
    }
    
    componentDidMount() {
        this.getGame()
        console.log("hello");
    }

    async getGame() {
        let games = await Games.getGames();
        console.log(games)
    }
}