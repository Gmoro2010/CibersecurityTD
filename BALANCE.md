# Balance de Cibersecurity TD

Los dos mapas comienzan con **50 monedas y 200 de vida**. El Mapa 1 tiene **12 bases** y entrega 3 monedas al cerrar una oleada no final; el Mapa 2 tiene **10 bases** y entrega 8. El premio de cierre no depende de que todos los enemigos hayan muerto; los enemigos que llegan al núcleo no dan monedas. La venta devuelve el 60 % de lo invertido, redondeado hacia abajo. Al combinar se conserva la inversión de ambas torres y no se cobra un recargo.

El Mapa 1 permite cubrir varios tramos del camino con una misma torre bien situada. El Mapa 2 tiene dos entradas y un tramo común más corto: se puede defender cerca de la unión o repartir el fuego antes y después. El láser castiga depender de una sola torre, aunque esté bien ubicada.

## Torres

El daño es por proyectil. La cadencia es en salvas por segundo y las velocidades en unidades de Unity por segundo. Una salva de 12 del tóxico reparte los tiros radialmente: no equivale a 12 impactos sobre un mismo enemigo.

| Torre | Costo | Rango | Daño | Cadencia | Proyectiles por salva | Velocidad de bala |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Cañón | 22 | 0,82 | 33 | 1,28 | 1 | 3,65 |
| Lanzallamas | 18 | 0,57 | 5 | 8,50 | 1 | 2,10 |
| Robot eléctrico | 27 | 0,74 | 16 | 1,55 | 2 | 3,65 |
| Robot tóxico | 28 | 0,67 | 20 | 1,80 | 12 | 1,95 |
| Cañón + eléctrico | 49 | 1,01 | 31 | 1,35 | 2 | 3,90 |
| Cañón + lanzallamas | 40 | 0,88 | 36 | 2,05 | 1 | 3,40 |
| Cañón + tóxico | 50 | 0,93 | 43 | 1,40 | 1 | 2,90 |
| Eléctrico + lanzallamas | 45 | 0,73 | 5 | 8,80 | 2 | 2,10 |
| Eléctrico + tóxico | 55 | 0,84 | 20 | 2,65 | 2 | 1,95 |
| Lanzallamas + tóxico | 46 | 0,68 | 11 | 5,60 | 1 | 2,30 |

El Cañón + tóxico tiene un radio de explosión de 0,23. El Lanzallamas mantiene la cadencia alta y el Robot tóxico conserva sus doce direcciones de disparo.

## Enemigos

| Enemigo | Vida | Velocidad | Daño al núcleo | Monedas al eliminarlo |
| --- | ---: | ---: | ---: | ---: |
| Bicho virus | 27 | 0,72 | 9 | 1 |
| Virus | 66 | 0,49 | 15 | 2 |
| Calavera | 154 | 0,36 | 24 | 2 |
| Trollface | 350 | 0,31 | 45 | 4 |
| USB | 60 | 0,78 | 22 | 2 |
| USB rojo | 96 | 0,84 | 30 | 3 |
| Anonymous | 6800 | 0,285 | 200 | 12 |
| Jefe láser | 3350 | 0,17 | 200 | 12 |

Anonymous invoca un Trollface cada 6 s mientras esté activo. El jefe láser desactiva la torre ocupada más cercana durante 5 s cada 7,5 s. Ambos cambian de sprite al 50 % de vida. El USB rojo tiene el 58 % de la escala lineal anterior.

## Oleadas

Formato: `cantidad × enemigo @ segundos entre apariciones`. El valor entre paréntesis es la espera desde la última aparición del grupo anterior. En el Mapa 2, `N` y `S` identifican las entradas norte y sur. Los totales excluyen las invocaciones de Anonymous.

### Mapa 1

| Oleada | Total | Grupos en orden |
| ---: | ---: | --- |
| 1 | 6 | 6 × virus @ 1,05 |
| 2 | 10 | 8 × virus @ 0,75; 2 × calavera @ 1,30 (0,80) |
| 3 | 11 | 6 × virus @ 0,62; 5 × calavera @ 0,82 (0,55) |
| 4 | 20 | 4 × calavera @ 0,70; 12 × virus @ 0,39 (0,55); 4 × calavera @ 0,70 (0,60) |
| 5 | 12 | 1 × troll; 8 × calavera @ 0,48 (0,70); 3 × troll @ 0,62 (0,60) |
| 6 | 29 | 3 × troll @ 0,57; 15 × virus @ 0,27 (0,40); 11 × calavera @ 0,39 (0,50) |
| 7 | 30 | 9 × calavera @ 0,30; 6 × troll @ 0,48 (0,45); 8 × USB @ 0,29 (0,40); 7 × calavera @ 0,31 (0,48) |
| 8 | 50 | 5 × troll @ 0,43; 16 × virus @ 0,19 (0,30); 15 × calavera @ 0,29 (0,37); 10 × USB @ 0,24 (0,40); 4 × troll @ 0,49 (0,43) |
| 9 | 52 | 6 × troll @ 0,38; 16 × calavera @ 0,25 (0,40); 8 × troll @ 0,42 (0,38); 10 × USB @ 0,24 (0,40); 12 × calavera @ 0,28 (0,45) |
| 10 | 43 | Anonymous; 12 × calavera @ 0,33 (1,20); 8 × troll @ 0,40 (0,43); 16 × virus @ 0,22 (0,45); 6 × troll @ 0,45 (0,80) |

### Mapa 2

| Oleada | Total | Grupos en orden |
| ---: | ---: | --- |
| 1 | 14 | N: 5 × bicho @ 0,65; S: 5 × virus @ 0,72 (0,60); S: 4 × bicho @ 0,52 (0,55) |
| 2 | 14 | S: 2 × calavera @ 1,20; N: 5 × bicho @ 0,49 (0,70); S: 3 × USB @ 1,00 (1,25); N: 4 × bicho @ 0,55 (1,00) |
| 3 | 13 | N: 3 × calavera @ 0,78; S: 2 × USB rojo @ 1,15 (1,10); N: 8 × bicho @ 0,39 (0,95) |
| 4 | 13 | S: 4 × calavera @ 0,98; N: 6 × USB @ 0,65 (0,85); S: 3 × USB rojo @ 0,90 (0,95) |
| 5 | 25 | N: 2 × troll @ 1,10; S: 6 × calavera @ 0,55 (0,47); N: 12 × bicho @ 0,27 (0,47); S: 5 × USB rojo @ 0,72 (0,65) |
| 6 | 30 | S: 3 × troll @ 0,65; N: 7 × USB rojo @ 0,39 (0,45); S: 8 × calavera @ 0,42 (0,60); N: 12 × bicho @ 0,22 (0,40) |
| 7 | 31 | N: 10 × USB rojo @ 0,33; S: 4 × troll @ 0,56 (0,45); N: 8 × USB @ 0,26 (0,45); S: 9 × calavera @ 0,42 (0,45) |
| 8 | 46 | S: 6 × troll @ 0,43; N: 19 × bicho @ 0,17 (0,36); S: 12 × calavera @ 0,30 (0,36); N: 9 × USB rojo @ 0,34 (0,37) |
| 9 | 44 | N: 7 × troll @ 0,40; S: 13 × USB rojo @ 0,30 (0,35); N: 13 × calavera @ 0,29 (0,40); S: 11 × USB @ 0,25 (0,38) |
| 10 | 41 | S: jefe láser; N: 10 × USB rojo @ 0,32 (1,30); S: 12 × calavera @ 0,37 (0,52); N: 8 × troll @ 0,46 (0,45); S: 10 × USB rojo @ 0,34 (0,47) |

## Verificación en Unity

- Mapa 1: ganaron dos planes completos, uno con cañón y eléctrico y otro con lanzallamas y tóxico. Ambos terminaron con 200/200 de vida.
- Mapa 2: ganaron cinco planes completos, tres con 200/200, uno con 71/200 y otro con 14/200. Entre ellos: abrir con dos cañones en el tramo común, defender la unión con tóxico y lanzallamas, y repartir eléctrico y cañón antes y después de la unión.
- En ambos mapas, sin construir se perdió en la oleada 2; dejando solo las dos torres iniciales se perdió en la oleada 5.
- También se comprobó el arranque del tutorial, sus dos oleadas y la combinación guiada, las pantallas de resultado, la venta, las dos formas de combinar y el cambio de sprite de ambos jefes a mitad de vida. El láser apuntó a la torre más cercana a los 7,5 s, liberó el bloqueo tras 5 s y volvió a disparar a los 15 s.
