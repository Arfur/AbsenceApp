param()

$e = "=" * 79
$d = "-" * 79

function Add-Header {
    param([string]$Path, [string]$FileName, [string]$Namespace, [string]$Purpose,
          [string]$Description = "", [string]$Notes = "", [string]$Version = "1.0.0")

    $content = Get-Content $Path -Raw
    if ($content -match "^/\*") { Write-Host "SKIP (already has header): $FileName"; return }

    $descBlock = ""
    if ($Description) {
        $descBlock = " Description :`n$Description`n$d`n"
    }
    $notesBlock = ""
    if ($Notes) {
        $notesBlock = " Notes       :`n$Notes`n$e`n"
    } else {
        $notesBlock = "$e`n"
    }

    $header = "/*`n$e`n File        : $FileName`n Namespace   : $Namespace`n Author      : Michael`n Version     : $Version`n Created     : 2026-03-15`n Updated     : 2026-03-15`n$d`n Purpose     : $Purpose`n$d`n${descBlock} Changes     :`n   - $Version  2026-03-15  Initial creation.`n$d`n$notesBlock*/`n`n"

    [System.IO.File]::WriteAllText($Path, ($header + $content), [System.Text.Encoding]::UTF8)
    Write-Host "DONE: $FileName"
}

# ─── Mappers ──────────────────────────────────────────────────────────────────

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Data\Mappers\StudentMapper.cs" `
    -FileName "StudentMapper.cs" `
    -Namespace "AbsenceApp.Data.Mappers" `
    -Purpose "Maps between the Student EF entity (TABLE29) and the Student`n               domain model (AbsenceApp.Core.Models.Student).`n               Resolves the naming collision between the two Student classes`n               using explicit type aliases." `
    -Notes "   - Uses CoreStudent / DataStudent aliases to avoid CS0104 ambiguity.`n   - YearGroup on the domain model carries the YearGroupId string for now;"

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Data\Mappers\UserMapper.cs" `
    -FileName "UserMapper.cs" `
    -Namespace "AbsenceApp.Data.Mappers" `
    -Purpose "Maps between the User EF entity (TABLE1 schema) and UserDto.`n               FirstName in the DTO carries the combined Name field from the entity.`n               IsActive is derived from the Status string property." `
    -Notes "   - UserId cast from long to int; current data fits within int range.`n   - Align UserDto to long once the client layer is updated."

# ─── Repositories ─────────────────────────────────────────────────────────────

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Data\Repositories\AttendanceRepository.cs" `
    -FileName "AttendanceRepository.cs" `
    -Namespace "AbsenceApp.Data.Repositories" `
    -Purpose "Legacy stub implementation of IAttendanceRepository.`n               Operates on the retained Attendance stub entity via Set<T>().`n               Full re-implementation targeting AttendanceRegister / AttendanceMark`n               is pending." `
    -Notes "   - Uses _context.Set<Attendance>() because the DbSet property for the`n     legacy Attendance type was not added to AppDbContext.`n   - Remove this stub once attendance UI targets the new register entities."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Data\Repositories\AuditLogRepository.cs" `
    -FileName "AuditLogRepository.cs" `
    -Namespace "AbsenceApp.Data.Repositories" `
    -Purpose "Legacy stub implementation of IAuditLogRepository.`n               Operates on the retained AuditLog stub entity via Set<T>().`n               Full audit trail is now distributed across five dedicated audit tables." `
    -Notes "   - Uses _context.Set<AuditLog>() for the same reason as AttendanceRepository.`n   - Remove this stub once all audit UI targets the per-entity audit tables."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Data\Repositories\RoleRepository.cs" `
    -FileName "RoleRepository.cs" `
    -Namespace "AbsenceApp.Data.Repositories" `
    -Purpose "Legacy stub implementation of IRoleRepository.`n               Operates on the retained Role stub entity via Set<T>().`n               Active role logic uses RoleType (TABLE6)." `
    -Notes "   - Uses _context.Set<Role>() for the same reason as AttendanceRepository.`n   - Remove this stub once all role UI targets RoleType."

# ─── Services ─────────────────────────────────────────────────────────────────

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Data\Services\ClassService.cs" `
    -FileName "ClassService.cs" `
    -Namespace "AbsenceApp.Data.Services" `
    -Purpose "EF Core implementation of IClassService.`n               Provides full CRUD operations for Class entities, mapping between`n               the Class EF entity and ClassDto via ClassMapper." `
    -Notes "   - Each mutating operation delegates SaveChanges to the repository.`n   - ClassName validation throws ArgumentException on blank value."

# ─── Test files ───────────────────────────────────────────────────────────────

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\MapperTests\ClassMapperTests.cs" `
    -FileName "ClassMapperTests.cs" `
    -Namespace "AbsenceApp.Tests.MapperTests" `
    -Purpose "xUnit tests for ClassMapper using the TABLE9 Class entity schema.`n               Verifies round-trip and field-level mappings between Class and ClassDto."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\MapperTests\StudentMapperTests.cs" `
    -FileName "StudentMapperTests.cs" `
    -Namespace "AbsenceApp.Tests.MapperTests" `
    -Purpose "xUnit tests for StudentMapper verifying correct field projection`n               between the Data-layer Student entity (TABLE29) and the Core domain model."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\MapperTests\UserMapperTests.cs" `
    -FileName "UserMapperTests.cs" `
    -Namespace "AbsenceApp.Tests.MapperTests" `
    -Purpose "xUnit tests for UserMapper using the TABLE1 User entity schema.`n               Verifies name combination, status-to-bool, and round-trip mappings."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\ServiceTests\ClassServiceTests.cs" `
    -FileName "ClassServiceTests.cs" `
    -Namespace "AbsenceApp.Tests.ServiceTests" `
    -Purpose "xUnit tests for ClassService using EF Core InMemory provider.`n               Covers GetAll, GetById, Create, Update, Delete, and validation errors."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\RepositoryTests\ClassRepositoryTests.cs" `
    -FileName "ClassRepositoryTests.cs" `
    -Namespace "AbsenceApp.Tests.RepositoryTests" `
    -Purpose "xUnit tests for ClassRepository using EF Core InMemory provider.`n               Verifies full CRUD contract and IQueryable composition against the`n               TABLE9 Class entity schema."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\RepositoryTests\UserRepositoryTests.cs" `
    -FileName "UserRepositoryTests.cs" `
    -Namespace "AbsenceApp.Tests.RepositoryTests" `
    -Purpose "xUnit tests for UserRepository using EF Core InMemory provider.`n               Verifies full CRUD contract and IQueryable composition against the`n               TABLE1 User entity schema."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\RepositoryTests\AttendanceRepositoryTests.cs" `
    -FileName "AttendanceRepositoryTests.cs" `
    -Namespace "AbsenceApp.Tests.RepositoryTests" `
    -Purpose "xUnit tests for AttendanceRepository (legacy stub) using EF Core InMemory.`n               Verifies the stub satisfies the IAttendanceRepository contract with`n               the retained Attendance entity; seeds users via the new TABLE1 schema."

Add-Header `
    -Path "C:\DevAbsence1\src\AbsenceApp.Tests\RepositoryTests\AuditLogRepositoryTests.cs" `
    -FileName "AuditLogRepositoryTests.cs" `
    -Namespace "AbsenceApp.Tests.RepositoryTests" `
    -Purpose "xUnit tests for AuditLogRepository (legacy stub) using EF Core InMemory.`n               Verifies the stub satisfies the IAuditLogRepository contract with`n               the retained AuditLog entity; seeds users via the new TABLE1 schema."

Write-Host "`nAll done."
