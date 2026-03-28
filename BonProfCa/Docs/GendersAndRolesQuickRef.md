# Genders and Roles API - Quick Reference

## ?? Endpoints

### Get All Genders
```
GET /genders/all
```
Returns list of all genders (Male, Female, Other)

### Get All Roles  
```
GET /roles/all
```
Returns list of all roles (SuperAdmin, Admin, Teacher, Student)

---

## ?? Response Format

Both endpoints return the same response structure:

```json
{
  "status": 200,
  "message": "Success message",
  "data": [...],
  "count": 3
}
```

---

## ?? Quick Usage

### JavaScript/TypeScript
```javascript
// Get genders
const gendersResponse = await fetch('/genders/all');
const { data: genders } = await gendersResponse.json();

// Get roles
const rolesResponse = await fetch('/roles/all');
const { data: roles } = await rolesResponse.json();
```

### C# (from another service)
```csharp
// Inject services
public MyService(GendersService gendersService, RolesService rolesService)
{
    var genders = await gendersService.GetAllGendersAsync();
    var roles = await rolesService.GetAllRolesAsync();
}
```

---

## ?? Data Structure

### Gender
- Id (Guid)
- Name (string)
- Color (string)
- Icon (string?)

### Role
- Id (Guid)
- Name (string)
- Color (string)
- CreatedAt (DateTimeOffset)
- UpdatedAt (DateTimeOffset?)

---

## ? Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 500 | Server Error |

---

## ?? Files Created

- `BonProf/Services/GendersService.cs`
- `BonProf/Services/RolesService.cs`
- `BonProf/Controllers/GendersController.cs`
- `BonProf/Controllers/RolesController.cs`
- `BonProf/Program.cs` (updated)

---

See `GendersAndRolesImplementation.md` for full documentation.
