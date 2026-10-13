# Práctica resuelta — Arquitectura por capas, DTOs y Controllers

Resolución completa de las nueve etapas, **con la inyección de dependencias ya aplicada**. Sirve como referencia para corregir o para mostrar el estado final.

## Cómo ejecutarlo

Abrir `ABMProductos.slnx` y apretar **F5**. Se abre Swagger solo, en el puerto 7406.

También se puede probar todo desde `ABMProductos.http`, que tiene las catorce requests listas para disparar.

## Estructura

```
/Controllers
    ProductsController.cs
/Services
    /Implementations   ProductService.cs
    /Interfaces        IProductService.cs
/Repositories
    /Implementations   ProductRepository.cs
    /Interfaces        IProductRepository.cs
/Entities
    Product.cs
/Models/DTOs
    /Responses         ProductForReadDto.cs, ProductStatsDto.cs
    /Requests          ProductForCreateDto.cs, ProductForUpdateDto.cs
Program.cs
```

## Los siete endpoints

| Verbo | Ruta | Respuestas | Etapa |
|---|---|---|---|
| `GET` | `/api/products` | `200` | 5 |
| `GET` | `/api/products/{id}` | `200`, `404` | 5 |
| `POST` | `/api/products` | `201`, `400`, `409` | 5 y 8 |
| `PUT` | `/api/products/{id}` | `204`, `400`, `404` | 5 y 8 |
| `DELETE` | `/api/products/{id}` | `204`, `404` | 5 |
| `GET` | `/api/products/search?name=` | `200`, `400` | 7 |
| `GET` | `/api/products/stats` | `200` | 7 |

## Resultados verificados

| Prueba | Respuesta |
|---|---|
| `GET /api/products/3` | `200` — Monitor 24 pulgadas |
| `GET /api/products/999` | `404` |
| `GET /api/products/search?name=tecla` | `200` — Teclado mecánico |
| `GET /api/products/search?name=zzz` | `200` — `[]` |
| `GET /api/products/stats` | `200` — 10 productos, promedio 54189.64, más caro Notebook Dell Inspiron |
| `POST` con nombre vacío y precio `-100` | `400` con el objeto `errors` |
| `POST` con nombre repetido | `409` |
| `POST` válido | `201` con el `Id` asignado |
| `PUT /api/products/999` | `404` |
| `PUT /api/products/1` | `204` |
| `DELETE /api/products/2` | `204` |
| `DELETE /api/products/2` otra vez | `404` |

---

## Decisiones que conviene señalar al corregir

**El controller pregunta antes de actualizar y borrar.** Como `UpdateProduct` y `DeleteProduct` del service devuelven `void`, no informan si el producto existía. El controller llama primero a `GetProductById` y devuelve `404` si da `null`.

**La búsqueda sin coincidencias devuelve `200` con lista vacía**, no `404`. El endpoint existe y respondió bien; que no haya resultados es un resultado válido.

**La búsqueda sin texto devuelve `400`.** Es distinto de no encontrar nada: acá la request está mal armada.

**`ProductStatsDto` no representa ninguna entidad.** Existe solo para darle forma a esa respuesta.

**Los dos casos de lista vacía están contemplados.** `AddProduct` usa `_products.Count == 0 ? 1 : ...` antes del `Max`, y `GetStats` devuelve ceros en lugar de llamar a `Average` sobre una lista sin elementos. Son los dos lugares donde el programa se cortaría.

**La regla del nombre repetido está en el service**, no en el controller, porque es una regla de negocio. El controller solo la consulta con `NameIsTaken` y traduce el `true` a un `409`.

> **Un detalle sobre esa regla:** `NameIsTaken` filtra en memoria con `GetAllProducts()`. Con una lista da igual, pero con una base de datos real eso significaría traer la tabla entera para comparar. En ese caso correspondería un método del repositorio que filtre del lado de la base.

**El orden de los endpoints en el controller importa poco pero conviene:** `search` y `stats` están escritos antes que `{id}`. ASP.NET Core da prioridad a los segmentos literales igual, así que funcionaría en cualquier orden, pero leerlo agrupado ayuda.

---

## Para mostrar en clase

**El paso de `new` a inyección de dependencias.** Comentar las dos líneas de `Program.cs` y ejecutar: aparece el error `Unable to resolve service for type 'IProductService' while attempting to activate 'ProductsController'`. Es la forma más rápida de mostrar qué hace cada registro.

**Que la validación corta antes del controller.** Poner un breakpoint en la primera línea de `Create` y mandar el `POST` con el nombre vacío: el breakpoint no se activa.

**Que el DTO oculta campos.** Acá la entidad y el DTO de lectura tienen los mismos tres campos, así que no se nota. Agregarle a `Product` una propiedad `CostPrice`, cargarla en la lista, y comprobar que **no aparece en el JSON** porque `ProductForReadDto` no la tiene.
