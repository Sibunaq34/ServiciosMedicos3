# React + Vite

## CORE2 y CORE7

CORE7 consulta CORE2 por medio del Gateway oficial en `MicroServicios/Gateway/`.

Configuracion local sugerida:

```env
VITE_API_BASE_URL=http://localhost:5220
```

Rutas relevantes:

- CORE6: `/puestos`
- CORE7: `/puestos/:codigoPuesto/oferentes`
- CORE2 por Gateway: `/api/v1/puestos/{codigoPuesto}/oferentes?page=1&pageSize=10`
- CORE9 provisional: `/oferentes/{idOferente}?codigoPuesto={codigoPuesto}`

Dependencias pendientes:

- CORE9 debe implementar la conversion del oferente seleccionado.
- La autenticacion queda bajo el flujo integrado de CORE4/CORE5.
- La coleccion Postman de CORE2 esta en `Postman/CORE2_Oferentes_Puesto.postman_collection.json`.

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Oxc](https://oxc.rs)
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/)

## React Compiler

The React Compiler is enabled on this template. See [this documentation](https://react.dev/learn/react-compiler) for more information.

Note: This will impact Vite dev & build performances.

## Expanding the ESLint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and [`typescript-eslint`](https://typescript-eslint.io) in your project.
