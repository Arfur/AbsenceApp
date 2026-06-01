using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenceApp.Data.Migrations
{
    /*
    ============================================================
    Migration  : RenameDesignTokensToNewNamingStandard
    Date       : 2026-06-01
    Author     : Michael
    ============================================================
    Applies the canonical design token naming standard:
        --ds-{component}-{variant}-{property}-{state?}

    Rules enforced:
      - property BEFORE state  (bg-hover, not hover-bg)
      - missing variant segments added (base, structure)
      - component group renames: form-field→input, form-shell→input-shell
      - dd ComponentGroup renamed to dropdown + CssVariable prefix updated
      - badge-status CssVariable prefix corrected: --ds-badge-* → --ds-badge-status-*

    Sections:
      1A  BTN  state-before-property   (6 rows)
      1B  BTN  structure tokens        (4 rows)
      1C  BTN  light/dark/outline-dark TokenKey normalise (25 rows)
      2   CARD missing base/structure  (5 rows)
      3   FORM-FIELD → INPUT           (11 rows, ComponentGroup renamed)
      4   FORM-SHELL → INPUT-SHELL     (5 rows, ComponentGroup renamed)
      5   NAV-SIDEBAR state fixes      (3 rows)
      6   TABLE base/state fixes       (4 rows)
      7   ALERT structure tokens       (2 rows)
      8   ICON-BTN structure + state   (12 rows)
      9   BADGE-STATUS prefix fix      (8 rows)
     10   ACTION-BTN state + structure (5 matching rows)
     11   DD → DROPDOWN                (16 rows, ComponentGroup + CssVariable prefix)

    NOTE: Sections 10 and 11 plan entries that reference tokens absent
    from the live DB (action-btn radius/font-size-sm/lg/icon-size;
    dropdown structural renames whose old values don't exist) are omitted.
    ============================================================
    */

    /// <inheritdoc />
    public partial class RenameDesignTokensToNewNamingStandard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── §1A: BTN state-before-property (6 rows) ──────────────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 13,  columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-primary-bg-hover",   "primary-bg-hover"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 23,  columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-secondary-bg-hover", "secondary-bg-hover" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 33,  columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-success-bg-hover",   "success-bg-hover"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 43,  columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-danger-bg-hover",    "danger-bg-hover"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 53,  columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-warning-bg-hover",   "warning-bg-hover"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 63,  columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-info-bg-hover",      "info-bg-hover"      });

            // ── §1B: BTN structure tokens (4 rows) ───────────────────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 70, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-structure-radius",    "structure-radius"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 71, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-structure-font-size", "structure-font-size" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 72, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-structure-padding-y", "structure-padding-y" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 73, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-btn-structure-padding-x", "structure-padding-x" });

            // ── §1C: BTN light/dark/outline-dark — TokenKey only (25 rows) ───────
            // CssVariable is already correctly formed; only TokenKey is normalised.
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 106, column: "TokenKey", value: "light-bg");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 107, column: "TokenKey", value: "light-text");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 108, column: "TokenKey", value: "light-border");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 109, column: "TokenKey", value: "light-bg-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 110, column: "TokenKey", value: "light-text-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 111, column: "TokenKey", value: "light-border-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 112, column: "TokenKey", value: "light-bg-disabled");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 113, column: "TokenKey", value: "light-text-disabled");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 114, column: "TokenKey", value: "light-border-disabled");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 115, column: "TokenKey", value: "dark-bg");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 116, column: "TokenKey", value: "dark-text");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 117, column: "TokenKey", value: "dark-border");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 118, column: "TokenKey", value: "dark-bg-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 119, column: "TokenKey", value: "dark-text-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 120, column: "TokenKey", value: "dark-border-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 121, column: "TokenKey", value: "dark-bg-disabled");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 122, column: "TokenKey", value: "dark-text-disabled");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 123, column: "TokenKey", value: "dark-border-disabled");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 124, column: "TokenKey", value: "outline-dark-text");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 125, column: "TokenKey", value: "outline-dark-border");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 126, column: "TokenKey", value: "outline-dark-bg-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 127, column: "TokenKey", value: "outline-dark-text-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 128, column: "TokenKey", value: "outline-dark-border-hover");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 129, column: "TokenKey", value: "outline-dark-text-disabled");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 130, column: "TokenKey", value: "outline-dark-border-disabled");

            // ── §2: CARD missing base/structure variant (5 rows) ─────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 100, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-card-base-bg",          "base-bg"          });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 101, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-card-base-border",      "base-border"      });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 102, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-card-structure-radius", "structure-radius" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 103, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-card-structure-shadow", "structure-shadow" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 105, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-card-structure-padding", "structure-padding" });

            // ── §3: FORM-FIELD → INPUT, ComponentGroup renamed (11 rows) ─────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 650, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-bg",           "base-bg"           });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 651, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-text",         "base-text"         });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 652, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-border",       "base-border"       });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 653, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-placeholder",  "base-placeholder"  });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 654, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-border-focus", "base-border-focus" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 655, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-ring-focus",   "base-ring-focus"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 656, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-border-error", "base-border-error" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 657, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-text-error",   "base-text-error"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 658, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-label",        "base-label"        });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 659, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-hint",         "base-hint"         });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 660, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input", "--ds-input-base-bg-disabled",  "base-bg-disabled"  });

            // ── §4: FORM-SHELL → INPUT-SHELL, ComponentGroup renamed (5 rows) ────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 700, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input-shell", "--ds-input-shell-bg",         "bg"         });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 701, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input-shell", "--ds-input-shell-border",     "border"     });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 702, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input-shell", "--ds-input-shell-shadow",     "shadow"     });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 703, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input-shell", "--ds-input-shell-text-muted", "text-muted" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 704, columns: new[] { "ComponentGroup", "CssVariable", "TokenKey" }, values: new object[] { "input-shell", "--ds-input-shell-action-bg",  "action-bg"  });

            // ── §5: NAV-SIDEBAR state fixes (3 rows) ─────────────────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 604, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-nav-sidebar-bg-hover",    "bg-hover"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 605, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-nav-sidebar-bg-active",   "bg-active"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 606, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-nav-sidebar-text-active", "text-active" });

            // ── §6: TABLE base/state fixes (4 rows) ──────────────────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 753, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-table-row-bg-hover",    "row-bg-hover"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 754, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-table-base-border",     "base-border"     });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 755, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-table-base-surface",    "base-surface"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 756, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-table-base-text-muted", "base-text-muted" });

            // ── §7: ALERT structure tokens (2 rows) ──────────────────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 812, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-alert-structure-shadow", "structure-shadow" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 813, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-alert-structure-radius", "structure-radius" });

            // ── §8: ICON-BTN structure + state (12 rows) ─────────────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 900, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-gap",          "structure-gap"          });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 901, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-radius",       "structure-radius"       });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 902, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-font-size-sm", "structure-font-size-sm" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 903, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-font-size-md", "structure-font-size-md" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 904, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-font-size-lg", "structure-font-size-lg" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 905, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-padding-sm",   "structure-padding-sm"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 906, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-padding-md",   "structure-padding-md"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 907, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-structure-padding-lg",   "structure-padding-lg"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 909, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-ghost-bg-hover",         "ghost-bg-hover"         });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 911, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-primary-bg-hover",       "primary-bg-hover"       });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 913, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-danger-bg-hover",        "danger-bg-hover"        });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 914, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-icon-btn-base-text",              "base-text"              });

            // ── §9: BADGE-STATUS CssVariable prefix fix (8 rows) ─────────────────
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 950, column: "CssVariable", value: "--ds-badge-status-active-bg");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 951, column: "CssVariable", value: "--ds-badge-status-active-text");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 952, column: "CssVariable", value: "--ds-badge-status-inactive-bg");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 953, column: "CssVariable", value: "--ds-badge-status-inactive-text");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 954, column: "CssVariable", value: "--ds-badge-status-planned-bg");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 955, column: "CssVariable", value: "--ds-badge-status-planned-text");
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 956, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-badge-status-structure-radius",    "structure-radius"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 957, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-badge-status-structure-font-size", "structure-font-size" });

            // ── §10: ACTION-BTN state + structure (5 matching rows) ──────────────
            // Note: plan entries for radius, font-size-sm, font-size-lg, icon-size,
            //       icon-only-size are omitted — those TokenKeys don't exist in the live DB.
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1013, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-action-btn-primary-bg-hover",    "primary-bg-hover"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1016, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-action-btn-secondary-bg-hover",  "secondary-bg-hover"  });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1021, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-action-btn-structure-padding-y", "structure-padding-y" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1022, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-action-btn-structure-padding-x", "structure-padding-x" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1023, columns: new[] { "CssVariable", "TokenKey" }, values: new object[] { "--ds-action-btn-structure-font-size", "structure-font-size" });

            // ── §11: DD → DROPDOWN — ComponentGroup + CssVariable prefix (16 rows) ─
            // The plan's structural/state renames for this section target IDs 1200–1215
            // which are absent from the live DB; only the ComponentGroup and CssVariable
            // prefix are updated here.
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1100, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-trigger-border-radius"  });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1101, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-trigger-padding-y"      });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1102, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-trigger-padding-x"      });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1103, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-trigger-font-size"      });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1104, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-bg"               });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1105, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-border"           });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1106, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-border-radius"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1107, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-shadow"           });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1108, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-item-text"        });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1109, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-item-hover-bg"    });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1110, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-item-active-bg"   });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1111, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-item-active-text" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1112, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-item-disabled-text" });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1113, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-padding-y"        });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1114, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-header-text"      });
            migrationBuilder.UpdateData(table: "DesignTokens", keyColumn: "Id", keyValue: 1115, columns: new[] { "ComponentGroup", "CssVariable" }, values: new object[] { "dropdown", "--ds-dropdown-menu-item-px"          });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Down intentionally left empty — this rename migration is not reversible.
        }
    }
}
