import * as React from "react";
import * as Games from "../remoting/Games";

export interface GameProps { compiler: string; framework: string; }

// 'HelloProps' describes the shape of props.
// State is never set so we use the 'undefined' type.
export class GameList extends React.Component<GameProps, undefined> {
    public render() {
        return <h1>Nothing is working! {this.props.compiler} and {this.props.framework}!</h1>;
    }

    public componentDidMount() {
        
    }
}