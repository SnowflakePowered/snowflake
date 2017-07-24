/**
 * Represents the lack of props.
 *
 * Actually types the well known symbol [Symbol.unscopables] to void as a prop.
 * This can be considered a noop, as Symbol.unscopables exists on
 * every JavaScript object and is unwritable, and unaccessable in a JSX declaration.
 *
 * This is in order to force TypeScript to infer the proper null type when using HOCs with
 * prop-less components. Without NoProps, typing the returned injector function to <{}> is required.
 *
 * Note that this is indeed a hack to abuse the TypeScript type-checker, however, does not
 * affect the compiled JavaScript after type-erasure. Once TypeScript allows support for
 * user-defined Symbols in type declarations, a custom Symbol will be used instead of the
 * built-in unscopables.
 *
 * If for some reason you need to use this symbol when writing your React component, first re-evaluate
 * your design. An alternative to NoProps is to provide the <{}> generic type to HOCs.
 */
export type NoProps = { [Symbol.unscopables]: void }
