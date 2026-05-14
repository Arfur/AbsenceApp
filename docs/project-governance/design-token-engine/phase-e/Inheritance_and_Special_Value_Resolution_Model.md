# Inheritance & Special-Value Resolution Model

- Phase: E4
- Date: 2026-05-15
- Status: Approved

## Objective
Define deterministic resolution behavior for inherited and special-value semantics in both synthesis and parsing paths.

## Resolution Order
1. Local explicit semantic intent
2. Direct inherited semantic intent
3. Transitive inherited intent
4. Component-level default intent
5. Terminal schema fallback intent

## Special-Value Rules
1. Special-value behavior is valid only when schema-declared.
2. Undeclared special-value behavior is invalid.
3. Resolution must be identical across synthesis and parsing policy contexts.

## Invalid Conditions
1. Cyclic inheritance
2. Missing inheritance reference
3. Competing equal-precedence intents
4. Scope-incompatible special-value declarations

## Gate Result
- E4: Closed
