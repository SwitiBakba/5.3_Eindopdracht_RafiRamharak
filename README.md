# PassTheWord

PassTheWord is een .NET console-applicatie voor het genereren van wachtwoorden met hoge entropie.  
De originele code is gerefactord zodat de applicatie beter leesbaar, beter testbaar en makkelijker uitbreidbaar is.

## Doel van de refactoring

De originele implementatie had veel logica in één methode. Hierdoor was de code lastig te lezen, moeilijk te testen en moeilijk uit te breiden.

De refactoring verdeelt de verantwoordelijkheden over meerdere klassen:

- `PasswordService` stuurt het proces aan.
- `PasswordOptions` bevat alle instellingen.
- `PasswordOptionsValidator` controleert of de instellingen geldig zijn.
- `RandomPasswordStrategy` genereert normale random wachtwoorden.
- `DictionaryPasswordStrategy` genereert wachtwoorden op basis van woorden.
- `CharacterReplacementService` past tekenvervangingen toe.
- `IAlphabet` maakt het mogelijk om nieuwe alfabetten toe te voegen.
- `IExternalPasswordVerifier` maakt externe wachtwoordcontrole mogelijk.

## Gebruikte design patterns

### Strategy Pattern

Gebruikt in:

- `IPasswordGenerationStrategy`
- `RandomPasswordStrategy`
- `DictionaryPasswordStrategy`

Er zijn meerdere manieren om een wachtwoord te genereren. Daarom is gekozen voor het Strategy Pattern.  
Nieuwe generatie-algoritmes kunnen worden toegevoegd zonder de bestaande strategieën aan te passen.

### Factory Pattern

Gebruikt in:

- `PasswordGenerationFactory`

De factory kiest welke generation strategy gebruikt moet worden.  
Hierdoor hoeft `PasswordService` niet zelf te weten welke concrete strategy nodig is.

### Facade Pattern

Gebruikt in:

- `PasswordService`

`PasswordService` verbergt de stappen van het proces:

1. opties valideren;
2. juiste strategy kiezen;
3. wachtwoord genereren;
4. replacements toepassen;
5. externe validatie uitvoeren.

Daardoor blijft `Program.cs` simpel en overzichtelijk.

### Adapter Point

Gebruikt in:

- `IExternalPasswordVerifier`
- `ExternalPasswordValidationService`

De opdracht vraagt om een koppeling voor externe wachtwoordvalidatie.  
Met `IExternalPasswordVerifier` kan later een externe dienst worden aangesloten zonder dat de rest van de applicatie aangepast hoeft te worden.

De applicatie geeft niet het wachtwoord zelf door, maar berekent eerst een hash.  
De externe verifier bepaalt daarna op basis van de hash of het wachtwoord veilig is.

## Alfabetten uitbreiden

Nieuwe alfabetten kunnen worden toegevoegd door `IAlphabet` te implementeren.

Voorbeeld:

```csharp
public class CyrillicAlphabet : IAlphabet
{
    public string Name => "Cyrillic";

    public string UppercaseCharacters => "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    public string LowercaseCharacters => "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
}
