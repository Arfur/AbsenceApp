# Canonical Entity Matrix

- Phase: A2
- Date: 2026-05-14
- Status: Approved

| Entity | Purpose | Mandatory Attributes | Optional Attributes | Notes |
|---|---|---|---|---|
| Component | Top-level domain container | componentKey, displayName, status | description, sortOrder | Uniqueness at global scope |
| Variant | Behavioral/style branch within component | variantKey, componentKey, displayName, status | description, sortOrder, inheritsFrom | Uniqueness within component |
| Property | Token-bearing attribute | propertyKey, componentKey, valueType, defaultValue, category | description, sortOrder, unitHint, editabilityHint | May be variant-scoped by policy |
| Variant Property Binding | Resolves property applicability to variant | componentKey, variantKey, propertyKey | overrideDefault, visibilityHint | Required for deterministic editor rendering |
| Inheritance Reference | Declares variant inheritance linkage | componentKey, variantKey, inheritsFromVariantKey | inheritanceMode | Cycles prohibited |
| Special-Value Semantic | Declares special behavior intent | semanticKey, applicableScope, interpretationRule | displayHint, validationHint | Must be explicit and typed |
| Editor Group | Drives editor grouping/order | groupKey, componentKey, label, order | description | UI-agnostic metadata only |
| Governance Metadata | Lifecycle and audit envelope | schemaVersion, compatibilityClass, approver, approvedAt | rationale, ticketRef | Mandatory for change acceptance |

## Deterministic Rules
1. Every mandatory attribute must be present for entity validity.
2. Missing mandatory attributes fail validation.
3. Optional attributes cannot change mandatory semantics.

## Gate Result
- A2: Closed
