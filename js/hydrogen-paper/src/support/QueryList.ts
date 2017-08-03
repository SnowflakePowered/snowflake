type QueryList = { [key: string]: string | string[] }

class QueryListHandler implements ProxyHandler<QueryList> {

  public get (target, property, receiver) {
    return target[property] || ''
  }

  public set (target, property, value, receiver) {
    return false
  }
}

const QueryList = (query: QueryList): QueryList => {
  const handler = new QueryListHandler()
  return new Proxy<QueryList>(query, handler)
}

export default QueryList
