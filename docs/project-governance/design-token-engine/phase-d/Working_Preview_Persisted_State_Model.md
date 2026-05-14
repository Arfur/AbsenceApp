# Working/Preview/Persisted State Model

- Phase: D4
- Date: 2026-05-15
- Status: Approved

## State Domains
1. Working (draft)
2. Preview (transient visualized intent)
3. Persisted (authoritative saved intent)

## Transition Rules
1. View -> Edit enters working state.
2. Edit -> Preview derives transient preview state from working state.
3. Edit/Preview -> Save commits to persisted state under validation pass.
4. Edit/Preview -> Cancel reverts working/preview and preserves persisted state.
5. Reset restores schema-governed baseline per policy.

## Deterministic Constraints
1. State transitions are explicit and unidirectional per event contract.
2. Persisted state cannot be mutated by preview-only transitions.
3. Ambiguous transition requests are invalid.

## Conflict Handling
1. Conflicts are classified under validation/feedback standards.
2. Invalid transitions are blocked and surfaced.

## Gate Result
- D4: Closed
