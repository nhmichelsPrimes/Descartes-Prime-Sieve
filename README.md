# Descartes-Prime-Sieve
Deterministic Descartes-based sieve for generating prime candidates z = p₁² − 12n².


# 📘 **README.md — Descartes Prime Sieve**

```markdown
# Descartes Prime Sieve
A deterministic candidate generator for primes of the form  
**z = p₁² − 12n²**, derived from integer Descartes triples and Eisenstein norms.

Descartes-basiertes Primzahlsieb (Prime Geometry Project).  
Für eine gegebene Primzahl `p₁` findet das Programm alle zulässigen Kandidaten `(p₁, p₂, z, a, b, d, e)`  
entlang der quadratischen Form:

```

p₂ = p₁ + 4n
z  = p₁² − 12n² ≥ 0
z  = d² − d e + e²   (Eisenstein norm form)
z  = a² + b²         (Gaussian form)

```

Alle erzeugten Kandidaten erfüllen die algebraischen Bedingungen, die notwendig sind, damit `z` *überhaupt* eine Primzahl sein kann.

---

# 🚀 Purpose

Der **Descartes-Sieve** dient als:

- schneller Kandidaten-Generator für die Werte `z`,
- geometrischer Vorfilter für den Einsatz weiterer Tests,
- deterministischer Teil des übergeordneten "Complete Prime Sieve" Projekts.

Das Ziel ist es, für ein gegebenes `p₁` **alle zulässigen integeren Konfigurationen** zu finden, die aus einem Descartes-Tripel `(k₁, k₂, k₃)` entstehen können.

---

# Mathematical Background

Der Algorithmus basiert auf folgenden Identitäten:

### 1. Descartes Triple Relation  
Für ein Descartes-Triple `(k₁, k₂, k₃)` gilt:

```

d = k₁ − k₂
e = k₁ − k₃
z = d² − d e + e²

```

Dies ist die **Eisenstein-Norm**,  
repräsentiert durch `N(d + e ω)` im Ring `ℤ[ω]`, ω = e^{2πi/3}.

### 2. Verbindung zu p₁ und p₂

```

p₁ = k₁ + k₂ + k₃
p₂ = p₁ + 4n
n² = (k₁ k₂ + k₁ k₃ + k₂ k₃) / 4

```

Dadurch entsteht automatisiert eine vollständige Abbildung

```

p₁  →  { alle zulässigen z }

```

allein durch das Durchsuchen der integeren Tripel.

---

# Code Overview

Der Kern befindet sich in:

```

SieveCandidateBuilder.FindCandidatesForP1(long p1)

````

Diese Funktion:

1. bestimmt die maximale zulässige Hyperbelweite `nMax = p₁ / √3`,
2. iteriert über `n`,
3. berechnet `z = p₁² − 12n²`,
4. rekonstruiert mögliche Descartes-Tripel `(k₁, k₂, k₃)`,
5. filtert sie nach:
   - GCD-Filter in `ℤ[ω]`,
   - Perfect-Power-Filter,
   - eindeutige Darstellung,
6. konstruiert gültige Kandidaten `SieveCandidate`.

---

# Implemented Filters

Der Kandidatenraum wird stark reduziert durch:

### 1. Modulo-4 Restklassensystem  
Jedes `kᵢ` liegt exakt in einer der zulässigen Restklassen. (Außerhalb dieser Restklassen gibt es keine Lösungen; auch keine nicht primen).

### 2. Quadratic Equation Filter  
`k₂` entsteht aus einer quadratischen Gleichung und wird nur akzeptiert,  
wenn `Δ` ein perfektes Quadrat ist. Das wird allerdings bereits durch die Generierung und die Descartes-Beziehungen sichergestellt.

### 3. Eisenstein GCD Filter  
Wenn `gcd(d, e) > 1`, kann `N(d + e ω)` **nicht** prim sein.

### ✔ 4. Perfect-Power Filter  
Verhindert Kandidaten der Form `pᵏ` mit `p ≥ 13`.
Den gcd-Filter wirkt auch auf `pᵏ`:

gcd(a+bω,d+eω) = π^min(r,r′) π^min(s,s′)
wenn min(r,r′)=0 und min(s,s′)=0
dann gcd(a+bω,d+eω) = 1

Dann ist Normseitig nicht von einer echten Primzahl zu unterscheiden.
Das sind die Fälle, die durchkommen. Das ist sehr selten.
---

# Data Structures

### SieveCandidate

```csharp
new SieveCandidate(
    p1,          // input prime
    p2,          // p1 + 4n
    n,           // geometric parameter
    z,           // candidate prime
    k1, k2, k3,  // Descartes triple
    quad         // (a, b, d, e) Norm representations
)
````

---

# Example Usage

```csharp
var candidates = SieveCandidateBuilder.FindCandidatesForP1(157);

foreach (var c in candidates)
{
    Console.WriteLine($"{c.P1} -> z={c.Z} via (k1,k2,k3)=({c.K1},{c.K2},{c.K3})");
}
```

---

# Output

Das Programm erzeugt je nach Einbettung:

* vollständige Kandidatenliste
* Descartes-Tripel
* Eisenstein-Normkomponenten
* Summanden für die a² + b² Darstellung

Ein typischer Output-Eintrag:

```
p1 = 157
n  = 19
z  = 157² − 12·19² = 18449 − 4332 = 14117
Descartes triple = (k1,k2,k3) = (43,47,67)
Eisenstein norm: d= -4, e=-24  → z = d² − d e + e²
```

---

# Testing

Kernelemente:

* Hyperbelgleichung wird exakt validiert
* Integer-Tripel werden eindeutig identifiziert
* Eisenstein-Filter arbeitet deterministisch
* Perfect-Power-Filter erkennt alle potenziellen pᵏ


---

# Contribution

Pull Requests sind willkommen — insbesondere:

* effizientere GCD-Filter
* bessere integer square root Methoden
* Parallelisierung
* mathematische Erweiterungen




-------------------------------------

IT License

Copyright (c) 2025 Norman-Hendrik Michels

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
