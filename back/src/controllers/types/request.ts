import {Request} from "express";


// @ts-ignore
interface Uri<Query, Body> extends Request {
    body: Body,
    query: Query
}

type User = {
    name: string
}


export type Login = Uri<{}, User & {
    hash?: string
}>

type Token = {
    token: string
}

export type IsValid = Uri<{}, Token>

export type DeleteToken = Uri<{}, User>


export type Clientify<U extends Uri<any, any>> = { body: U["body"], query: U["query"] }



