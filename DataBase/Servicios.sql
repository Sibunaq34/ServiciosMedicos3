-- phpMyAdmin SQL Dump
-- version 5.2.3
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost:3306
-- Tiempo de generación: 18-08-2026 a las 18:23:09
-- Versión del servidor: 10.5.29-MariaDB
-- Versión de PHP: 8.4.22

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `Servicios`
--
CREATE DATABASE IF NOT EXISTS `Servicios` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `Servicios`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `accion_personal`
--

CREATE TABLE `accion_personal` (
  `id_accion` int(11) NOT NULL,
  `codigo_accion` varchar(20) DEFAULT NULL,
  `fecha_accion` date DEFAULT NULL,
  `descripcion` varchar(500) DEFAULT NULL,
  `id_empleado` int(11) DEFAULT NULL,
  `id_jefactura` int(11) DEFAULT NULL,
  `fecha_creacion` date DEFAULT NULL,
  `fehca_modificacion` date DEFAULT NULL,
  `activo` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `accion_personal`
--

INSERT INTO `accion_personal` (`id_accion`, `codigo_accion`, `fecha_accion`, `descripcion`, `id_empleado`, `id_jefactura`, `fecha_creacion`, `fehca_modificacion`, `activo`) VALUES
(1, 'CON-0001', '2026-01-05', 'Contratación de la coordinadora de Recursos Humanos.', 1, 2, '2026-01-05', NULL, 1),
(2, 'CON-0002', '2026-01-10', 'Contratación del director médico.', 2, NULL, '2026-01-10', NULL, 1),
(3, 'CON-0003', '2026-02-02', 'Contratación de médica general para consulta externa.', 3, 2, '2026-02-02', NULL, 1),
(4, 'CON-0004', '2026-02-15', 'Contratación de enfermero profesional.', 4, 2, '2026-02-15', NULL, 1),
(5, 'AJU-0001', '2026-06-01', 'Ajuste salarial anual por evaluación satisfactoria.', 3, 2, '2026-06-01', NULL, 1),
(6, 'TRA-0001', '2026-07-01', 'Traslado del empleado al servicio de consulta general.', 3, 2, '2026-07-01', NULL, 1),
(14, 'CON-12', '2026-07-21', 'Contratación de empleado', 12, NULL, '2026-07-21', NULL, 1),
(15, 'CON-13', '2026-07-21', 'Contratación de empleado', 13, NULL, '2026-07-21', NULL, 1),
(16, 'CON-14', '2026-07-21', 'Contratación de empleado', 14, NULL, '2026-07-21', NULL, 1),
(17, 'CON-15', '2026-07-21', 'Contratación de empleado', 15, NULL, '2026-07-21', NULL, 1),
(18, 'CON-16', '2026-07-21', 'Contratación de empleado', 16, NULL, '2026-07-21', NULL, 1),
(19, 'CON-17', '2026-07-21', 'Contratación de empleado', 17, NULL, '2026-07-21', NULL, 1),
(20, 'CON-18', '2026-07-21', 'Contratación de empleado', 18, NULL, '2026-07-21', NULL, 1),
(21, 'CON-19', '2026-07-26', 'Contratación de empleado', 19, NULL, '2026-07-26', NULL, 1),
(22, 'CON-20', '2026-07-27', 'Contratación de empleado', 20, NULL, '2026-07-27', NULL, 1),
(23, 'CON-21', '2026-08-18', 'Contratación de empleado', 21, NULL, '2026-08-18', NULL, 1),
(24, 'CON-22', '2026-08-18', 'Contratación de empleado', 22, NULL, '2026-08-18', NULL, 1),
(25, 'CON-23', '2026-08-18', 'Contratación de empleado', 23, NULL, '2026-08-18', NULL, 1),
(26, 'CON-24', '2026-08-18', 'Contratación de empleado', 24, NULL, '2026-08-18', NULL, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `admin_area`
--

CREATE TABLE `admin_area` (
  `id_area` int(11) NOT NULL,
  `codigo_area` varchar(20) DEFAULT NULL,
  `nombre_area` varchar(100) DEFAULT NULL,
  `id_empleado` int(11) DEFAULT NULL,
  `activon` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `admin_area`
--

INSERT INTO `admin_area` (`id_area`, `codigo_area`, `nombre_area`, `id_empleado`, `activon`) VALUES
(1, 'AREA-RH', 'Recursos Humanos', 1, 1),
(2, 'AREA-DM', 'Dirección Médica', 2, 1),
(3, 'AREA-CG', 'Consulta General', 3, 1),
(4, 'AREA-ENF', 'Enfermería', 4, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `bitacoras`
--

CREATE TABLE `bitacoras` (
  `id_bitacoras` int(11) NOT NULL,
  `fecha_bitacora` datetime NOT NULL DEFAULT current_timestamp(),
  `id_usuario` int(11) NOT NULL,
  `accion` varchar(50) NOT NULL,
  `descripcionAccion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `bitacoras`
--

INSERT INTO `bitacoras` (`id_bitacoras`, `fecha_bitacora`, `id_usuario`, `accion`, `descripcionAccion`) VALUES
(1, '2026-07-20 23:28:39', 2, 'Crear AUT3', '{\"mensaje\": \"Se registra oferente publico para puesto\", \"registro\": {\"idPersona\": \"13\", \"idOferente\": \"13\", \"idPuesto\": \"2\", \"idOferentePuesto\": \"9\", \"identificacion\": \"305280498\", \"tipoIdentificacion\": \"CedulaIdentidad\", \"nombreCompleto\": \"Antony Cervantes Calderon\", \"codigoPuesto\": \"RH-COOR\", \"nombrePuesto\": \"Coordinador de Recursos Humanos\", \"correos\": [\"antonny22c.c@gmail.com\"], \"telefonos\": [\"63368175\"], \"curriculum\": {\"ruta\": \"aut3-curriculums/aut3_20260721_052839_aaf495e007cf09e9.pdf\", \"nombre\": \"Resumen_Antony_Cervantes.pdf\", \"mime\": \"application/pdf\", \"tamanio\": \"207583\"}}}'),
(2, '2026-07-26 21:28:05', 2, 'Crear AUT3', '{\"mensaje\": \"Se registra oferente publico para puesto\", \"registro\": {\"idPersona\": \"14\", \"idOferente\": \"14\", \"idPuesto\": \"6\", \"idOferentePuesto\": \"10\", \"identificacion\": \"305280499\", \"tipoIdentificacion\": \"CedulaIdentidad\", \"nombreCompleto\": \"ANTONY CERVANTES C\", \"codigoPuesto\": \"ADM-ASI\", \"nombrePuesto\": \"Asistente Administrativo\", \"correos\": [\"GEOVANNY22C.C@GMAIL.COM\"], \"telefonos\": [\"62494245\"], \"curriculum\": {\"ruta\": \"aut3-curriculums/aut3_20260727_032805_d37e1be48c35da92.pdf\", \"nombre\": \"Resumen_Antony_Cervantes.pdf\", \"mime\": \"application/pdf\", \"tamanio\": \"207583\"}}}'),
(3, '2026-07-26 23:34:45', 2, 'Crear AUT3', '{\"mensaje\": \"Se registra oferente publico para puesto\", \"registro\": {\"idPersona\": \"15\", \"idOferente\": \"15\", \"idPuesto\": \"2\", \"idOferentePuesto\": \"11\", \"identificacion\": \"305280422\", \"tipoIdentificacion\": \"CedulaIdentidad\", \"nombreCompleto\": \"Geovany Cervantes Calderon\", \"codigoPuesto\": \"RH-COOR\", \"nombrePuesto\": \"Coordinador de Recursos Humanos\", \"correos\": [\"mastersibunaq34@gmail.com\"], \"telefonos\": [\"63368175\"], \"curriculum\": {\"ruta\": \"aut3-curriculums/aut3_20260727_053445_914c608940916030.pdf\", \"nombre\": \"CV_Antony_Cervantes.pdf\", \"mime\": \"application/pdf\", \"tamanio\": \"187718\"}}}'),
(4, '2026-07-28 10:36:22', 2, 'Crear AUT3', '{\"mensaje\": \"Se registra oferente publico para puesto\", \"registro\": {\"idPersona\": \"16\", \"idOferente\": \"16\", \"idPuesto\": \"2\", \"idOferentePuesto\": \"12\", \"identificacion\": \"307280999\", \"tipoIdentificacion\": \"CedulaIdentidad\", \"nombreCompleto\": \"Luis Alejandro Mata\", \"codigoPuesto\": \"RH-COOR\", \"nombrePuesto\": \"Coordinador de Recursos Humanos\", \"correos\": [\"luis@gmail.com\"], \"telefonos\": [\"62494245\"], \"curriculum\": {\"ruta\": \"aut3-curriculums/aut3_20260728_163622_050e46c813e2763f.pdf\", \"nombre\": \"CV_Antony_Cervantes.pdf\", \"mime\": \"application/pdf\", \"tamanio\": \"187718\"}}}'),
(5, '2026-08-18 16:21:35', 2, 'Crear AUT3', '{\"mensaje\": \"Se registra oferente publico para puesto\", \"registro\": {\"idPersona\": \"17\", \"idOferente\": \"17\", \"idPuesto\": \"1\", \"idOferentePuesto\": \"13\", \"identificacion\": \"205280498\", \"tipoIdentificacion\": \"CedulaIdentidad\", \"nombreCompleto\": \"Gerardo Gutierrez\", \"codigoPuesto\": \"DIR-MED\", \"nombrePuesto\": \"Director Médico\", \"correos\": [\"ggutierrez@test.com\"], \"telefonos\": [\"63368175\"], \"curriculum\": {\"ruta\": \"aut3-curriculums/aut3_20260818_222137_4381054707d29579.pdf\", \"nombre\": \"CV_Antony_Cervantes.pdf\", \"mime\": \"application/pdf\", \"tamanio\": \"187718\"}}}'),
(6, '2026-08-18 16:41:42', 2, 'Crear AUT3', '{\"mensaje\": \"Se registra oferente publico para puesto\", \"registro\": {\"idPersona\": \"18\", \"idOferente\": \"18\", \"idPuesto\": \"6\", \"idOferentePuesto\": \"14\", \"identificacion\": \"305280123\", \"tipoIdentificacion\": \"CedulaIdentidad\", \"nombreCompleto\": \"Prueba\", \"codigoPuesto\": \"ADM-ASI\", \"nombrePuesto\": \"Asistente Administrativo\", \"correos\": [\"test@gmail.com\"], \"telefonos\": [\"63368175\"], \"curriculum\": {\"ruta\": \"aut3-curriculums/aut3_20260818_224144_9deb38e99b1f3c2c.pdf\", \"nombre\": \"CV_Antony_Cervantes.pdf\", \"mime\": \"application/pdf\", \"tamanio\": \"187718\"}}}'),
(7, '2026-08-18 17:18:34', 2, 'Crear AUT3', '{\"mensaje\": \"Se registra oferente publico para puesto\", \"registro\": {\"idPersona\": \"19\", \"idOferente\": \"19\", \"idPuesto\": \"6\", \"idOferentePuesto\": \"15\", \"identificacion\": \"407890991\", \"tipoIdentificacion\": \"CedulaIdentidad\", \"nombreCompleto\": \"Daniel Perez\", \"codigoPuesto\": \"ADM-ASI\", \"nombrePuesto\": \"Asistente Administrativo\", \"correos\": [\"dani@gmail.com\"], \"telefonos\": [\"63368175\"], \"curriculum\": {\"ruta\": \"aut3-curriculums/aut3_20260818_231837_46e88fd864b105dc.pdf\", \"nombre\": \"CV_Antony_Cervantes.pdf\", \"mime\": \"application/pdf\", \"tamanio\": \"187718\"}}}');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `canton`
--

CREATE TABLE `canton` (
  `id_canton` int(11) NOT NULL,
  `nombre` varchar(100) DEFAULT NULL,
  `id_provincia` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `canton`
--

INSERT INTO `canton` (`id_canton`, `nombre`, `id_provincia`) VALUES
(8, 'Belén', 4),
(1, 'Central', 1),
(3, 'Central', 2),
(5, 'Central', 3),
(7, 'Central', 4),
(11, 'Central', 6),
(13, 'Central', 7),
(2, 'Escazú', 1),
(12, 'Esparza', 6),
(6, 'La Unión', 3),
(9, 'Liberia', 5),
(10, 'Nicoya', 5),
(14, 'Pococí', 7),
(4, 'San Ramón', 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `companias`
--

CREATE TABLE `companias` (
  `id_compania` int(11) NOT NULL,
  `codigo_compania` varchar(50) NOT NULL,
  `nombre` varchar(150) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `companias`
--

INSERT INTO `companias` (`id_compania`, `codigo_compania`, `nombre`) VALUES
(1, 'SM-001', 'Servicios Médicos SA'),
(2, 'LAB-001', 'Laboratorios Clínicos del Este'),
(3, 'FAR-001', 'Farmacia Central Cartago'),
(4, 'SEG-001', 'Aseguradora Vida y Salud'),
(5, 'INS-001', 'Insumos Hospitalarios Costa Rica'),
(6, 'TEC-001', 'Tecnología Médica del Valle');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `concursos`
--

CREATE TABLE `concursos` (
  `id_concursos` int(11) NOT NULL,
  `codigo_concurso` varchar(30) NOT NULL,
  `nombre_concurso` varchar(150) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `estado_concur` enum('Vigente','Vencido') NOT NULL DEFAULT 'Vigente'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `concursos`
--

INSERT INTO `concursos` (`id_concursos`, `codigo_concurso`, `nombre_concurso`, `fecha_inicio`, `fecha_fin`, `estado_concur`) VALUES
(1, 'CON-2026-001', 'Concurso para Médico General', '2026-07-01', '2026-08-15', 'Vigente'),
(2, 'CON-2026-002', 'Concurso para Enfermero Profesional', '2026-07-05', '2026-08-20', 'Vigente'),
(3, 'CON-2026-003', 'Concurso para Técnico de Laboratorio', '2026-07-10', '2026-08-25', 'Vigente'),
(4, 'CON-2026-004', 'Concurso para Asistente Administrativo', '2026-07-12', '2026-08-10', 'Vigente'),
(5, 'CON-2026-005', 'Concurso para Técnico en Farmacia', '2026-07-15', '2026-08-30', 'Vigente'),
(6, 'CON-2026-006', 'Concurso para Coordinador de Recursos Humanos', '2026-05-01', '2026-06-15', 'Vencido'),
(7, 'CON-2026-007', 'Concurso para Auxiliar de Limpieza', '2026-04-01', '2026-05-10', 'Vencido'),
(8, 'CON-2026-008', 'Concurso para Director Médico', '2026-07-18', '2026-09-01', 'Vigente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `distrito`
--

CREATE TABLE `distrito` (
  `id_distrito` int(11) NOT NULL,
  `nombre` varchar(100) DEFAULT NULL,
  `id_canton` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `distrito`
--

INSERT INTO `distrito` (`id_distrito`, `nombre`, `id_canton`) VALUES
(5, 'Alajuela', 3),
(18, 'Cañas Dulces', 9),
(1, 'Carmen', 1),
(2, 'Catedral', 1),
(22, 'Chacarita', 11),
(3, 'Escazú', 2),
(23, 'Espíritu Santo', 12),
(27, 'Guápiles', 14),
(13, 'Heredia', 7),
(28, 'Jiménez', 14),
(16, 'La Ribera', 8),
(17, 'Liberia', 9),
(25, 'Limón', 13),
(20, 'Mansión', 10),
(14, 'Mercedes', 7),
(19, 'Nicoya', 10),
(10, 'Occidental', 5),
(9, 'Oriental', 5),
(21, 'Puntarenas', 11),
(15, 'San Antonio', 8),
(12, 'San Diego', 6),
(6, 'San José', 3),
(24, 'San Juan Grande', 12),
(4, 'San Rafael', 2),
(7, 'San Ramón', 4),
(8, 'Santiago', 4),
(11, 'Tres Ríos', 6),
(26, 'Valle La Estrella', 13);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleados`
--

CREATE TABLE `empleados` (
  `id_empleado` int(11) NOT NULL,
  `numero_empleado` varchar(20) DEFAULT NULL,
  `id_oferente` int(11) DEFAULT NULL,
  `fecha_creacion` date DEFAULT NULL,
  `nombre_completo` varchar(200) DEFAULT NULL,
  `identificacion` varchar(20) DEFAULT NULL,
  `tipo_identificacion` enum('CedulaIdentidad','DIMEX','Pasaporte') DEFAULT NULL,
  `id_puesto` int(11) DEFAULT NULL,
  `fecha_contratacion` date DEFAULT NULL,
  `estado` enum('activo','inactivo') DEFAULT NULL,
  `fecha_modificacion` date DEFAULT NULL,
  `id_usuario` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `empleados`
--

INSERT INTO `empleados` (`id_empleado`, `numero_empleado`, `id_oferente`, `fecha_creacion`, `nombre_completo`, `identificacion`, `tipo_identificacion`, `id_puesto`, `fecha_contratacion`, `estado`, `fecha_modificacion`, `id_usuario`) VALUES
(1, 'EMP-2026-0001', 1, '2026-01-05', 'Ana Sofía Vargas Rojas', '117650432', 'CedulaIdentidad', 2, '2026-01-05', 'activo', NULL, NULL),
(2, 'EMP-2026-0002', 2, '2026-01-10', 'Luis Fernando Mora Castro', '304980721', 'CedulaIdentidad', 1, '2026-01-10', 'activo', NULL, NULL),
(3, 'EMP-2026-0003', 3, '2026-02-02', 'Valeria Jiménez Araya', '109870654', 'CedulaIdentidad', 3, '2026-02-02', 'activo', NULL, NULL),
(4, 'EMP-2026-0004', 4, '2026-02-15', 'Andrés Mauricio Hernández López', '155812345678', 'DIMEX', 4, '2026-02-15', 'activo', NULL, NULL),
(12, 'EMP-20260721020528', 9, '2026-07-21', 'Camila Andrea Soto Peña', '155898765432', 'DIMEX', 6, '2026-07-21', 'activo', NULL, NULL),
(13, 'EMP-20260721020631', 5, '2026-07-21', 'Daniela María Solano Vega', '702340981', 'CedulaIdentidad', 6, '2026-07-21', 'activo', NULL, NULL),
(14, 'EMP-20260721021150', 12, '2026-07-21', 'Carlos Andrés Mora Solano', '118760945', 'CedulaIdentidad', 2, '2026-07-21', 'activo', NULL, NULL),
(15, 'EMP-20260721021648', 10, '2026-07-21', 'Ricardo Antonio López Marín', 'P87654321', 'Pasaporte', 7, '2026-07-21', 'activo', NULL, NULL),
(16, 'EMP-20260721021937', 8, '2026-07-21', 'José Pablo Quesada Brenes', '402780123', 'CedulaIdentidad', 5, '2026-07-21', 'activo', NULL, NULL),
(17, 'EMP-20260721022002', 6, '2026-07-21', 'Gabriel Esteban Rojas Méndez', 'P12345678', 'Pasaporte', 3, '2026-07-21', 'activo', NULL, NULL),
(18, 'EMP-20260721022110', 7, '2026-07-21', 'Natalia Fernanda Chaves Mora', '205670432', 'CedulaIdentidad', 4, '2026-07-21', 'activo', NULL, NULL),
(19, 'EMP-20260726233755', 15, '2026-07-26', 'Geovany Cervantes Calderon', '305280422', 'CedulaIdentidad', 2, '2026-07-26', 'activo', NULL, NULL),
(20, 'EMP-20260727000034', 13, '2026-07-27', 'Antony Cervantes Calderon', '305280498', 'CedulaIdentidad', 2, '2026-07-27', 'activo', NULL, NULL),
(21, 'EMP-20260818162911', 17, '2026-08-18', 'Gerardo Gutierrez', '205280498', 'CedulaIdentidad', 1, '2026-08-18', 'activo', NULL, NULL),
(22, 'EMP-20260818164812', 18, '2026-08-18', 'Prueba', '305280123', 'CedulaIdentidad', 6, '2026-08-18', 'activo', NULL, NULL),
(23, 'EMP-20260818164818', 14, '2026-08-18', 'ANTONY CERVANTES C', '305280499', 'CedulaIdentidad', 6, '2026-08-18', 'activo', NULL, NULL),
(24, 'EMP-20260818165526', 16, '2026-08-18', 'Luis Alejandro Mata', '307280999', 'CedulaIdentidad', 2, '2026-08-18', 'activo', NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `entrevistas`
--

CREATE TABLE `entrevistas` (
  `id_entrevista` int(11) NOT NULL,
  `id_oferente` int(11) NOT NULL,
  `id_empleado` int(11) NOT NULL,
  `fecha_entrevista` datetime NOT NULL,
  `estado` enum('Pendiente','Realizada') DEFAULT 'Pendiente'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `entrevistas`
--

INSERT INTO `entrevistas` (`id_entrevista`, `id_oferente`, `id_empleado`, `fecha_entrevista`, `estado`) VALUES
(1, 5, 1, '2026-07-15 09:00:00', 'Realizada'),
(2, 6, 2, '2026-07-16 10:30:00', 'Realizada'),
(3, 7, 4, '2026-07-18 14:00:00', 'Realizada'),
(4, 8, 2, '2026-07-22 08:30:00', 'Pendiente'),
(5, 9, 1, '2026-07-23 11:00:00', 'Pendiente'),
(6, 10, 2, '2026-07-24 13:30:00', 'Pendiente'),
(7, 11, 1, '2026-07-27 09:30:00', 'Pendiente'),
(8, 12, 1, '2026-07-28 15:00:00', 'Pendiente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `experiencia_laboral`
--

CREATE TABLE `experiencia_laboral` (
  `id_experiencia` int(11) NOT NULL,
  `id_oferente` int(11) NOT NULL,
  `nombre_empresa` varchar(100) NOT NULL,
  `puesto_desempenado` varchar(100) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `experiencia_laboral`
--

INSERT INTO `experiencia_laboral` (`id_experiencia`, `id_oferente`, `nombre_empresa`, `puesto_desempenado`, `fecha_inicio`, `fecha_fin`) VALUES
(1, 1, 'Centro Médico Los Ángeles', 'Analista de Recursos Humanos', '2015-01-05', '2020-12-18'),
(2, 1, 'Clínica Santa Elena', 'Coordinadora de Personal', '2021-01-04', '2025-11-28'),
(3, 2, 'Hospital Regional Cartago', 'Médico General', '2014-01-06', '2021-12-30'),
(4, 2, 'Clínica Integral Central', 'Jefe Médico', '2022-01-03', '2025-11-30'),
(5, 3, 'Área de Salud El Guarco', 'Médico General', '2021-01-04', '2025-12-20'),
(6, 4, 'Hospital Metropolitano', 'Enfermero de Emergencias', '2015-01-05', '2025-12-18'),
(7, 5, 'Consultorio Médico La Sabana', 'Asistente Administrativa', '2020-01-06', '2025-06-30'),
(8, 6, 'Clínica San Rafael', 'Médico de Consulta Externa', '2020-01-06', '2026-06-30'),
(9, 7, 'Hospital Monseñor Sanabria', 'Enfermera Profesional', '2021-02-01', '2026-06-28'),
(10, 8, 'Laboratorio Bioanálisis', 'Técnico de Laboratorio', '2016-01-04', '2026-06-30'),
(11, 9, 'Servicios Administrativos del Sur', 'Asistente Administrativa', '2018-01-08', '2026-06-30'),
(12, 10, 'Farmacia La Salud', 'Técnico en Farmacia', '2011-01-03', '2026-06-25'),
(13, 11, 'Clínica Familiar Cartago', 'Recepcionista', '2022-01-03', '2026-06-30'),
(14, 12, 'Hospital del Valle', 'Analista de Recursos Humanos', '2016-01-04', '2026-06-30');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `institu_educa`
--

CREATE TABLE `institu_educa` (
  `id_insti_edu` int(11) NOT NULL,
  `codigo_insti` varchar(30) NOT NULL,
  `nombre` varchar(150) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `institu_educa`
--

INSERT INTO `institu_educa` (`id_insti_edu`, `codigo_insti`, `nombre`) VALUES
(1, 'UCR', 'Universidad de Costa Rica'),
(2, 'UNA', 'Universidad Nacional de Costa Rica'),
(3, 'TEC', 'Instituto Tecnológico de Costa Rica'),
(4, 'UNED', 'Universidad Estatal a Distancia'),
(5, 'UTN', 'Universidad Técnica Nacional'),
(6, 'CUC', 'Colegio Universitario de Cartago'),
(7, 'INA', 'Instituto Nacional de Aprendizaje'),
(8, 'UCIMED', 'Universidad de Ciencias Médicas'),
(9, 'UH', 'Universidad Hispanoamericana'),
(10, 'ULATINA', 'Universidad Latina de Costa Rica');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `oferentes`
--

CREATE TABLE `oferentes` (
  `id_oferente` int(11) NOT NULL,
  `id_persona` int(11) NOT NULL,
  `fecha_regis` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `oferentes`
--

INSERT INTO `oferentes` (`id_oferente`, `id_persona`, `fecha_regis`) VALUES
(1, 1, '2025-12-01 09:00:00'),
(2, 2, '2025-12-02 09:15:00'),
(3, 3, '2026-01-05 10:20:00'),
(4, 4, '2026-01-08 11:00:00'),
(5, 5, '2026-07-02 08:30:00'),
(6, 6, '2026-07-03 09:10:00'),
(7, 7, '2026-07-05 13:45:00'),
(8, 8, '2026-07-07 15:20:00'),
(9, 9, '2026-07-09 10:05:00'),
(10, 10, '2026-07-11 14:35:00'),
(11, 11, '2026-07-14 08:50:00'),
(12, 12, '2026-07-18 16:15:00'),
(13, 13, '2026-07-20 23:28:39'),
(14, 14, '2026-07-26 21:28:05'),
(15, 15, '2026-07-26 23:34:45'),
(16, 16, '2026-07-28 10:36:22'),
(17, 17, '2026-08-18 16:21:35'),
(18, 18, '2026-08-18 16:41:42'),
(19, 19, '2026-08-18 17:18:34');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `oferente_concur`
--

CREATE TABLE `oferente_concur` (
  `id_of_concurso` int(11) NOT NULL,
  `id_oferente` int(11) NOT NULL,
  `id_concursos` int(11) NOT NULL,
  `fecha_asigna` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `oferente_concur`
--

INSERT INTO `oferente_concur` (`id_of_concurso`, `id_oferente`, `id_concursos`, `fecha_asigna`) VALUES
(1, 5, 4, '2026-07-02 08:35:00'),
(2, 5, 6, '2026-07-02 08:40:00'),
(3, 6, 1, '2026-07-03 09:15:00'),
(4, 6, 8, '2026-07-03 09:20:00'),
(5, 7, 2, '2026-07-05 13:50:00'),
(6, 8, 3, '2026-07-07 15:25:00'),
(7, 9, 4, '2026-07-09 10:10:00'),
(8, 10, 5, '2026-07-11 14:40:00'),
(9, 11, 4, '2026-07-14 08:55:00'),
(10, 12, 6, '2026-07-18 16:20:00'),
(11, 12, 4, '2026-07-18 16:25:00');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `oferente_correo`
--

CREATE TABLE `oferente_correo` (
  `id_of_correo` int(11) NOT NULL,
  `id_oferente` int(11) NOT NULL,
  `correo` varchar(150) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `oferente_correo`
--

INSERT INTO `oferente_correo` (`id_of_correo`, `id_oferente`, `correo`) VALUES
(1, 1, 'ana.vargas@correo.test'),
(2, 2, 'luis.mora@correo.test'),
(3, 3, 'valeria.jimenez@correo.test'),
(4, 4, 'andres.hernandez@correo.test'),
(5, 5, 'daniela.solano@correo.test'),
(6, 5, 'daniela.solano.laboral@correo.test'),
(7, 6, 'gabriel.rojas@correo.test'),
(8, 7, 'natalia.chaves@correo.test'),
(9, 8, 'jose.quesada@correo.test'),
(10, 9, 'camila.soto@correo.test'),
(11, 10, 'ricardo.lopez@correo.test'),
(12, 11, 'maria.arce@correo.test'),
(13, 12, 'carlos.mora@correo.test'),
(14, 13, 'antonny22c.c@gmail.com'),
(15, 14, 'GEOVANNY22C.C@GMAIL.COM'),
(16, 15, 'mastersibunaq34@gmail.com'),
(17, 16, 'luis@gmail.com'),
(18, 17, 'ggutierrez@test.com'),
(19, 18, 'test@gmail.com'),
(20, 19, 'dani@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `oferente_puesto`
--

CREATE TABLE `oferente_puesto` (
  `id_oferente_puesto` int(11) NOT NULL,
  `id_oferente` int(11) NOT NULL,
  `id_puesto` int(11) NOT NULL,
  `fecha_postulacion` datetime NOT NULL DEFAULT current_timestamp(),
  `estado` enum('Postulado','Cancelado') NOT NULL DEFAULT 'Postulado',
  `ruta_curriculum` varchar(500) NOT NULL,
  `nombre_curriculum` varchar(255) NOT NULL,
  `mime_curriculum` varchar(120) NOT NULL,
  `tamanio_curriculum` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `oferente_puesto`
--

INSERT INTO `oferente_puesto` (`id_oferente_puesto`, `id_oferente`, `id_puesto`, `fecha_postulacion`, `estado`, `ruta_curriculum`, `nombre_curriculum`, `mime_curriculum`, `tamanio_curriculum`) VALUES
(1, 5, 6, '2026-07-02 08:45:00', 'Postulado', 'uploads/curriculum/cv_daniela_solano.pdf', 'cv_daniela_solano.pdf', 'application/pdf', 184320),
(2, 6, 3, '2026-07-03 09:25:00', 'Postulado', 'uploads/curriculum/cv_gabriel_rojas.pdf', 'cv_gabriel_rojas.pdf', 'application/pdf', 215040),
(3, 7, 4, '2026-07-05 13:55:00', 'Postulado', 'uploads/curriculum/cv_natalia_chaves.pdf', 'cv_natalia_chaves.pdf', 'application/pdf', 176128),
(4, 8, 5, '2026-07-07 15:30:00', 'Postulado', 'uploads/curriculum/cv_jose_quesada.pdf', 'cv_jose_quesada.pdf', 'application/pdf', 194560),
(5, 9, 6, '2026-07-09 10:15:00', 'Postulado', 'uploads/curriculum/cv_camila_soto.pdf', 'cv_camila_soto.pdf', 'application/pdf', 168960),
(6, 10, 7, '2026-07-11 14:45:00', 'Postulado', 'uploads/curriculum/cv_ricardo_lopez.pdf', 'cv_ricardo_lopez.pdf', 'application/pdf', 205824),
(7, 11, 6, '2026-07-14 09:00:00', 'Postulado', 'uploads/curriculum/cv_maria_arce.pdf', 'cv_maria_arce.pdf', 'application/pdf', 157696),
(8, 12, 2, '2026-07-18 16:30:00', 'Postulado', 'uploads/curriculum/cv_carlos_mora.pdf', 'cv_carlos_mora.pdf', 'application/pdf', 221184),
(9, 13, 2, '2026-07-20 23:28:39', 'Postulado', 'aut3-curriculums/aut3_20260721_052839_aaf495e007cf09e9.pdf', 'Resumen_Antony_Cervantes.pdf', 'application/pdf', 207583),
(10, 14, 6, '2026-07-26 21:28:05', 'Postulado', 'aut3-curriculums/aut3_20260727_032805_d37e1be48c35da92.pdf', 'Resumen_Antony_Cervantes.pdf', 'application/pdf', 207583),
(11, 15, 2, '2026-07-26 23:34:45', 'Postulado', 'aut3-curriculums/aut3_20260727_053445_914c608940916030.pdf', 'CV_Antony_Cervantes.pdf', 'application/pdf', 187718),
(12, 16, 2, '2026-07-28 10:36:22', 'Postulado', 'aut3-curriculums/aut3_20260728_163622_050e46c813e2763f.pdf', 'CV_Antony_Cervantes.pdf', 'application/pdf', 187718),
(13, 17, 1, '2026-08-18 16:21:35', 'Postulado', 'aut3-curriculums/aut3_20260818_222137_4381054707d29579.pdf', 'CV_Antony_Cervantes.pdf', 'application/pdf', 187718),
(14, 18, 6, '2026-08-18 16:41:42', 'Postulado', 'aut3-curriculums/aut3_20260818_224144_9deb38e99b1f3c2c.pdf', 'CV_Antony_Cervantes.pdf', 'application/pdf', 187718),
(15, 19, 6, '2026-08-18 17:18:34', 'Postulado', 'aut3-curriculums/aut3_20260818_231837_46e88fd864b105dc.pdf', 'CV_Antony_Cervantes.pdf', 'application/pdf', 187718);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `oferente_requisito`
--

CREATE TABLE `oferente_requisito` (
  `id_oferente` int(11) NOT NULL,
  `id_requisito` int(11) NOT NULL,
  `cumple` tinyint(1) NOT NULL DEFAULT 1,
  `fecha_registro` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `oferente_requisito`
--

INSERT INTO `oferente_requisito` (`id_oferente`, `id_requisito`, `cumple`, `fecha_registro`) VALUES
(5, 15, 1, '2026-07-20 15:38:07'),
(5, 16, 1, '2026-07-20 15:38:07'),
(6, 7, 1, '2026-07-20 15:38:07'),
(6, 8, 1, '2026-07-20 15:38:07'),
(6, 9, 1, '2026-07-20 15:38:07'),
(7, 10, 1, '2026-07-20 15:38:07'),
(7, 11, 1, '2026-07-20 15:38:07'),
(7, 12, 1, '2026-07-20 15:38:07'),
(8, 13, 1, '2026-07-20 15:38:07'),
(8, 14, 1, '2026-07-20 15:38:07'),
(9, 15, 1, '2026-07-20 15:38:07'),
(9, 16, 1, '2026-07-20 15:38:07'),
(10, 17, 1, '2026-07-20 15:38:07'),
(10, 18, 1, '2026-07-20 15:38:07'),
(11, 15, 1, '2026-07-20 15:38:07'),
(11, 16, 1, '2026-07-20 15:38:07'),
(12, 4, 1, '2026-07-20 15:38:07'),
(12, 5, 1, '2026-07-20 15:38:07'),
(12, 6, 1, '2026-07-20 15:38:07');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `oferente_telf`
--

CREATE TABLE `oferente_telf` (
  `id_of_telefono` int(11) NOT NULL,
  `id_oferente` int(11) NOT NULL,
  `telefono` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `oferente_telf`
--

INSERT INTO `oferente_telf` (`id_of_telefono`, `id_oferente`, `telefono`) VALUES
(1, 1, '88881001'),
(2, 2, '88881002'),
(3, 3, '88881003'),
(4, 4, '88881004'),
(5, 5, '88881005'),
(6, 5, '22221005'),
(7, 6, '88881006'),
(8, 7, '88881007'),
(9, 8, '88881008'),
(10, 9, '88881009'),
(11, 10, '88881010'),
(12, 11, '88881011'),
(13, 12, '88881012'),
(14, 13, '63368175'),
(15, 14, '62494245'),
(16, 15, '63368175'),
(17, 16, '62494245'),
(18, 17, '63368175'),
(19, 18, '63368175'),
(20, 19, '63368175');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pantallas`
--

CREATE TABLE `pantallas` (
  `id_pantalla` int(11) NOT NULL,
  `nombre_pantalla` varchar(100) NOT NULL,
  `fecha_creacion` datetime DEFAULT current_timestamp(),
  `fecha_modificacion` datetime DEFAULT NULL,
  `activo` tinyint(4) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `pantallas`
--

INSERT INTO `pantallas` (`id_pantalla`, `nombre_pantalla`, `fecha_creacion`, `fecha_modificacion`, `activo`) VALUES
(1, 'Inicio', '2026-07-20 08:00:00', NULL, 1),
(2, 'Usuarios', '2026-07-20 08:00:00', NULL, 1),
(3, 'Roles', '2026-07-20 08:00:00', NULL, 1),
(4, 'Pantallas', '2026-07-20 08:00:00', NULL, 1),
(5, 'Parámetros', '2026-07-20 08:00:00', NULL, 1),
(6, 'Bitácora', '2026-07-20 08:00:00', NULL, 1),
(7, 'Cargar Datos de Ubicación', '2026-07-20 08:00:00', NULL, 1),
(8, 'Compañías', '2026-07-20 08:00:00', NULL, 1),
(9, 'Oferentes', '2026-07-20 08:00:00', NULL, 1),
(10, 'Concursos', '2026-07-20 08:00:00', NULL, 1),
(11, 'Agendar Entrevistas', '2026-07-20 08:00:00', NULL, 1),
(12, 'Contratar Empleado', '2026-07-20 08:00:00', NULL, 1),
(13, 'Puestos', '2026-07-20 08:00:00', NULL, 1),
(14, 'Áreas', '2026-07-20 08:00:00', NULL, 1),
(15, 'Acciones de Personal', '2026-07-20 08:00:00', NULL, 1),
(16, 'Instituciones Educativas', '2026-07-20 08:00:00', NULL, 1),
(17, 'Listado de Puestos Activos', '2026-07-20 08:00:00', NULL, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `parametros`
--

CREATE TABLE `parametros` (
  `id_parametro` int(11) NOT NULL,
  `codigo_parametro` varchar(100) NOT NULL,
  `valor` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `parametros`
--

INSERT INTO `parametros` (`id_parametro`, `codigo_parametro`, `valor`) VALUES
(1, 'LONGITUD_USUARIO', '50'),
(2, 'LONGITUD_NOMBRE_ROL', '40'),
(3, 'LONGITUD_NOMBRE_PANTALLA', '100'),
(4, 'LONGITUD_COD_AREA', '20'),
(5, 'LONGITUD_NOMBRE_AREA', '100'),
(6, 'LONGITUD_COD_COMPANIA', '50'),
(7, 'LONGITUD_NOMBRE_COMPANIA', '150'),
(8, 'LONGITUD_NOMBRE_PUESTO', '150'),
(9, 'LONGITUD_NOMBRE_INSTITUCION', '150'),
(10, 'MAXIMO_REGISTROS_PAGINA', '10'),
(11, 'MAXIMO_REGISTROS_BITACORA', '100'),
(12, 'INTENTOS_MAXIMOS_LOGIN', '3');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `personas`
--

CREATE TABLE `personas` (
  `id_persona` int(11) NOT NULL,
  `identificacion` varchar(30) NOT NULL,
  `tipo_identificacion` enum('CedulaIdentidad','DIMEX','Pasaporte') NOT NULL,
  `nombre_comple` varchar(150) NOT NULL,
  `fecha_naci` date NOT NULL,
  `tipo_perso` enum('Oferente','Empleado') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `personas`
--

INSERT INTO `personas` (`id_persona`, `identificacion`, `tipo_identificacion`, `nombre_comple`, `fecha_naci`, `tipo_perso`) VALUES
(1, '117650432', 'CedulaIdentidad', 'Ana Sofía Vargas Rojas', '1992-04-18', 'Empleado'),
(2, '304980721', 'CedulaIdentidad', 'Luis Fernando Mora Castro', '1988-09-03', 'Empleado'),
(3, '109870654', 'CedulaIdentidad', 'Valeria Jiménez Araya', '1995-12-11', 'Empleado'),
(4, '155812345678', 'DIMEX', 'Andrés Mauricio Hernández López', '1990-06-25', 'Empleado'),
(5, '702340981', 'CedulaIdentidad', 'Daniela María Solano Vega', '1998-02-14', 'Oferente'),
(6, 'P12345678', 'Pasaporte', 'Gabriel Esteban Rojas Méndez', '1994-07-30', 'Oferente'),
(7, '205670432', 'CedulaIdentidad', 'Natalia Fernanda Chaves Mora', '1997-10-09', 'Oferente'),
(8, '402780123', 'CedulaIdentidad', 'José Pablo Quesada Brenes', '1993-01-22', 'Oferente'),
(9, '155898765432', 'DIMEX', 'Camila Andrea Soto Peña', '1996-05-16', 'Oferente'),
(10, 'P87654321', 'Pasaporte', 'Ricardo Antonio López Marín', '1989-11-28', 'Oferente'),
(11, '603450987', 'CedulaIdentidad', 'María José Arce Salazar', '2000-03-07', 'Oferente'),
(12, '118760945', 'CedulaIdentidad', 'Carlos Andrés Mora Solano', '1991-08-19', 'Oferente'),
(13, '305280498', 'CedulaIdentidad', 'Antony Cervantes Calderon', '2000-11-12', 'Oferente'),
(14, '305280499', 'CedulaIdentidad', 'ANTONY CERVANTES C', '2000-11-12', 'Oferente'),
(15, '305280422', 'CedulaIdentidad', 'Geovany Cervantes Calderon', '2000-11-12', 'Oferente'),
(16, '307280999', 'CedulaIdentidad', 'Luis Alejandro Mata', '2000-11-11', 'Oferente'),
(17, '205280498', 'CedulaIdentidad', 'Gerardo Gutierrez', '2000-08-13', 'Oferente'),
(18, '305280123', 'CedulaIdentidad', 'Prueba', '2000-11-12', 'Oferente'),
(19, '407890991', 'CedulaIdentidad', 'Daniel Perez', '2000-11-12', 'Oferente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `prepara_academica`
--

CREATE TABLE `prepara_academica` (
  `id_pre_academica` int(11) NOT NULL,
  `id_oferente` int(11) NOT NULL,
  `id_insti_edu` int(11) NOT NULL,
  `titulo_obtenido` varchar(100) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Volcado de datos para la tabla `prepara_academica`
--

INSERT INTO `prepara_academica` (`id_pre_academica`, `id_oferente`, `id_insti_edu`, `titulo_obtenido`, `fecha_inicio`, `fecha_fin`) VALUES
(1, 1, 2, 'Bachillerato en Administración de Recursos Humanos', '2010-02-01', '2014-12-15'),
(2, 1, 4, 'Licenciatura en Gestión del Talento Humano', '2015-02-01', '2017-12-10'),
(3, 2, 1, 'Licenciatura en Medicina y Cirugía', '2007-02-01', '2013-12-15'),
(4, 2, 8, 'Maestría en Administración de Servicios de Salud', '2015-02-01', '2017-11-30'),
(5, 3, 8, 'Licenciatura en Medicina y Cirugía', '2014-01-15', '2020-12-10'),
(6, 4, 9, 'Licenciatura en Enfermería', '2009-02-01', '2014-12-12'),
(7, 5, 6, 'Diplomado en Administración de Empresas', '2017-02-01', '2019-12-10'),
(8, 6, 1, 'Licenciatura en Medicina y Cirugía', '2013-02-01', '2019-12-15'),
(9, 7, 8, 'Licenciatura en Enfermería', '2015-02-01', '2020-12-12'),
(10, 8, 3, 'Diplomado en Laboratorio Clínico', '2012-02-01', '2015-12-10'),
(11, 9, 5, 'Diplomado en Asistencia Administrativa', '2015-01-15', '2017-12-08'),
(12, 10, 7, 'Técnico en Farmacia', '2008-02-01', '2010-12-10'),
(13, 11, 6, 'Diplomado en Administración de Empresas', '2019-02-01', '2021-12-10'),
(14, 12, 2, 'Bachillerato en Recursos Humanos', '2011-02-01', '2015-12-10');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `provincias`
--

CREATE TABLE `provincias` (
  `id_provincia` int(11) NOT NULL,
  `nombre` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `provincias`
--

INSERT INTO `provincias` (`id_provincia`, `nombre`) VALUES
(2, 'Alajuela'),
(3, 'Cartago'),
(5, 'Guanacaste'),
(4, 'Heredia'),
(7, 'Limón'),
(6, 'Puntarenas'),
(1, 'San José');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `puestos`
--

CREATE TABLE `puestos` (
  `id_puesto` int(11) NOT NULL,
  `codigo_puesto` varchar(20) DEFAULT NULL,
  `nombre_puesto` varchar(150) DEFAULT NULL,
  `monto_salario` decimal(12,2) DEFAULT NULL,
  `id_puesto_jefac` int(11) DEFAULT NULL,
  `fecha_creacion` datetime DEFAULT NULL,
  `fecha_modificacion` datetime DEFAULT NULL,
  `activo` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `puestos`
--

INSERT INTO `puestos` (`id_puesto`, `codigo_puesto`, `nombre_puesto`, `monto_salario`, `id_puesto_jefac`, `fecha_creacion`, `fecha_modificacion`, `activo`) VALUES
(1, 'DIR-MED', 'Director Médico', 2200000.00, NULL, '2026-01-10 08:00:00', NULL, 1),
(2, 'RH-COOR', 'Coordinador de Recursos Humanos', 1200000.00, NULL, '2026-01-10 08:05:00', NULL, 1),
(3, 'MED-GEN', 'Médico General', 1350000.00, 1, '2026-01-10 08:10:00', NULL, 1),
(4, 'ENF-PRO', 'Enfermero Profesional', 950000.00, 1, '2026-01-10 08:15:00', NULL, 1),
(5, 'LAB-TEC', 'Técnico de Laboratorio', 780000.00, 1, '2026-01-10 08:20:00', NULL, 1),
(6, 'ADM-ASI', 'Asistente Administrativo', 650000.00, 2, '2026-01-10 08:25:00', NULL, 1),
(7, 'FAR-TEC', 'Técnico en Farmacia', 720000.00, 1, '2026-01-10 08:30:00', NULL, 1),
(8, 'LIM-AUX', 'Auxiliar de Limpieza', 480000.00, 6, '2026-01-10 08:35:00', '2026-07-01 09:00:00', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `requisitos_puesto`
--

CREATE TABLE `requisitos_puesto` (
  `id_requisito` int(11) NOT NULL,
  `id_puesto` int(11) DEFAULT NULL,
  `nombre_requisito` varchar(200) NOT NULL DEFAULT '',
  `fecha_creacion` datetime DEFAULT NULL,
  `fecha_modificacion` datetime DEFAULT NULL,
  `activo` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `requisitos_puesto`
--

INSERT INTO `requisitos_puesto` (`id_requisito`, `id_puesto`, `nombre_requisito`, `fecha_creacion`, `fecha_modificacion`, `activo`) VALUES
(1, 1, 'Licenciatura en Medicina y Cirugía', '2026-01-10 09:00:00', NULL, 1),
(2, 1, 'Incorporación vigente al Colegio de Médicos', '2026-01-10 09:01:00', NULL, 1),
(3, 1, 'Cinco años de experiencia en gestión clínica', '2026-01-10 09:02:00', NULL, 1),
(4, 2, 'Bachillerato en Recursos Humanos', '2026-01-10 09:03:00', NULL, 1),
(5, 2, 'Tres años de experiencia en reclutamiento', '2026-01-10 09:04:00', NULL, 1),
(6, 2, 'Conocimiento de legislación laboral costarricense', '2026-01-10 09:05:00', NULL, 1),
(7, 3, 'Licenciatura en Medicina y Cirugía', '2026-01-10 09:06:00', NULL, 1),
(8, 3, 'Incorporación vigente al Colegio de Médicos', '2026-01-10 09:07:00', NULL, 1),
(9, 3, 'Disponibilidad para turnos rotativos', '2026-01-10 09:08:00', NULL, 1),
(10, 4, 'Licenciatura en Enfermería', '2026-01-10 09:09:00', NULL, 1),
(11, 4, 'Incorporación vigente al Colegio de Enfermeras', '2026-01-10 09:10:00', NULL, 1),
(12, 4, 'Certificación vigente en soporte vital básico', '2026-01-10 09:11:00', NULL, 1),
(13, 5, 'Diplomado en Laboratorio Clínico', '2026-01-10 09:12:00', NULL, 1),
(14, 5, 'Experiencia en manejo de muestras biológicas', '2026-01-10 09:13:00', NULL, 1),
(15, 6, 'Bachillerato en Educación Media', '2026-01-10 09:14:00', NULL, 1),
(16, 6, 'Manejo de herramientas de oficina', '2026-01-10 09:15:00', NULL, 1),
(17, 7, 'Técnico en Farmacia', '2026-01-10 09:16:00', NULL, 1),
(18, 7, 'Conocimiento de control de inventarios', '2026-01-10 09:17:00', NULL, 1),
(19, 8, 'Experiencia en limpieza hospitalaria', '2026-01-10 09:18:00', '2026-07-01 09:00:00', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE `roles` (
  `id_rol` int(11) NOT NULL,
  `nombre_permiso` varchar(40) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `roles`
--

INSERT INTO `roles` (`id_rol`, `nombre_permiso`) VALUES
(1, 'Administrador'),
(2, 'Reclutador de personal');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `rolpantalla`
--

CREATE TABLE `rolpantalla` (
  `id_rol` int(11) NOT NULL,
  `id_pantalla` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `rolpantalla`
--

INSERT INTO `rolpantalla` (`id_rol`, `id_pantalla`) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5),
(1, 6),
(1, 7),
(1, 8),
(1, 9),
(1, 10),
(1, 11),
(1, 12),
(1, 13),
(1, 14),
(1, 15),
(1, 16),
(1, 17),
(2, 1),
(2, 9),
(2, 10),
(2, 11),
(2, 12),
(2, 13),
(2, 14),
(2, 15),
(2, 16),
(2, 17);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuariorol`
--

CREATE TABLE `usuariorol` (
  `id_usuario` int(11) NOT NULL,
  `id_rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `usuariorol`
--

INSERT INTO `usuariorol` (`id_usuario`, `id_rol`) VALUES
(1, 1),
(3, 1),
(4, 1),
(5, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL,
  `usuario` varchar(50) NOT NULL,
  `contrasena` varchar(255) NOT NULL,
  `activo` tinyint(4) NOT NULL,
  `fecha_modifi` datetime DEFAULT NULL,
  `fecha_access` datetime DEFAULT NULL,
  `nombre_completo` varchar(150) DEFAULT NULL,
  `correo` varchar(150) DEFAULT NULL,
  `estado` varchar(20) DEFAULT 'Activo',
  `intentos_fallidos` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id_usuario`, `usuario`, `contrasena`, `activo`, `fecha_modifi`, `fecha_access`, `nombre_completo`, `correo`, `estado`, `intentos_fallidos`) VALUES
(1, 'Admin', 'GCM:A7BgGuXsJmU/wr6FOhkvQQBjIKE+7eIK6Jz3NSowyehGpNypw9I=', 1, '2026-07-20 14:35:40', '2026-08-11 13:13:57', 'Antony Cervantes Calderon', 'antony22c.c@gmail.com', 'Activo', 0),
(2, 'AUT_PUBLICO', '5b8f2a74c4a3a21f8d9eb59ccd343bbf139b286335ef84ec103564035cab0e9a', 0, '2026-07-20 16:07:21', NULL, 'Usuario tecnico publico AUT3', NULL, 'Inactivo', 0),
(3, 'Admin2', 'GCM:1tJ7hlA1EPkHjF/2kpsdiXFaexKVW/O1xWtEwB7CwY8g/JxgvlY=', 0, '2026-07-20 23:48:33', '2026-07-20 23:48:13', 'Roberto ', 'adsas@sa.com', 'Bloqueado', 3),
(4, 'Admin1', 'GCM:86OzoeZUqRN4OtWUNM+TXIHbZnMLQBWrHUt04ZIMAUouzIFb794=', 1, '2026-08-18 15:58:49', '2026-08-18 17:09:16', 'Antony Cervantes Calderon', 'nuevo@test.com', 'Activo', 0),
(5, 'postman_prueba', 'GCM:S2jJ04SjPzANyJjJnGvKv6oYWmlVBirJlYvArbQvJqKThQgL5kQ8', 1, '2026-08-18 15:45:13', NULL, 'Usuario de prueba Postman', 'postman.prueba@example.com', 'Activo', 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `accion_personal`
--
ALTER TABLE `accion_personal`
  ADD PRIMARY KEY (`id_accion`),
  ADD KEY `id_empleado` (`id_empleado`),
  ADD KEY `id_jefactura` (`id_jefactura`);

--
-- Indices de la tabla `admin_area`
--
ALTER TABLE `admin_area`
  ADD PRIMARY KEY (`id_area`);

--
-- Indices de la tabla `bitacoras`
--
ALTER TABLE `bitacoras`
  ADD PRIMARY KEY (`id_bitacoras`),
  ADD KEY `id_usuario` (`id_usuario`);

--
-- Indices de la tabla `canton`
--
ALTER TABLE `canton`
  ADD PRIMARY KEY (`id_canton`),
  ADD UNIQUE KEY `nombre` (`nombre`,`id_provincia`),
  ADD KEY `id_provincia` (`id_provincia`);

--
-- Indices de la tabla `companias`
--
ALTER TABLE `companias`
  ADD PRIMARY KEY (`id_compania`),
  ADD UNIQUE KEY `uq_codigo_compania` (`codigo_compania`);

--
-- Indices de la tabla `concursos`
--
ALTER TABLE `concursos`
  ADD PRIMARY KEY (`id_concursos`),
  ADD UNIQUE KEY `codigo_concurso` (`codigo_concurso`);

--
-- Indices de la tabla `distrito`
--
ALTER TABLE `distrito`
  ADD PRIMARY KEY (`id_distrito`),
  ADD UNIQUE KEY `nombre` (`nombre`,`id_canton`),
  ADD KEY `id_canton` (`id_canton`);

--
-- Indices de la tabla `empleados`
--
ALTER TABLE `empleados`
  ADD PRIMARY KEY (`id_empleado`),
  ADD UNIQUE KEY `id_usuario` (`id_usuario`),
  ADD KEY `id_oferente` (`id_oferente`),
  ADD KEY `id_puesto` (`id_puesto`);

--
-- Indices de la tabla `entrevistas`
--
ALTER TABLE `entrevistas`
  ADD PRIMARY KEY (`id_entrevista`),
  ADD KEY `entrevistas_ibfk_1` (`id_oferente`),
  ADD KEY `entrevistas_ibfk_2` (`id_empleado`);

--
-- Indices de la tabla `experiencia_laboral`
--
ALTER TABLE `experiencia_laboral`
  ADD PRIMARY KEY (`id_experiencia`),
  ADD KEY `experiencia_laboral_ibfk_1` (`id_oferente`);

--
-- Indices de la tabla `institu_educa`
--
ALTER TABLE `institu_educa`
  ADD PRIMARY KEY (`id_insti_edu`),
  ADD UNIQUE KEY `codigo_insti` (`codigo_insti`);

--
-- Indices de la tabla `oferentes`
--
ALTER TABLE `oferentes`
  ADD PRIMARY KEY (`id_oferente`),
  ADD UNIQUE KEY `id_persona` (`id_persona`);

--
-- Indices de la tabla `oferente_concur`
--
ALTER TABLE `oferente_concur`
  ADD PRIMARY KEY (`id_of_concurso`),
  ADD UNIQUE KEY `id_oferente` (`id_oferente`,`id_concursos`),
  ADD KEY `id_concursos` (`id_concursos`);

--
-- Indices de la tabla `oferente_correo`
--
ALTER TABLE `oferente_correo`
  ADD PRIMARY KEY (`id_of_correo`),
  ADD KEY `id_oferente` (`id_oferente`);

--
-- Indices de la tabla `oferente_puesto`
--
ALTER TABLE `oferente_puesto`
  ADD PRIMARY KEY (`id_oferente_puesto`),
  ADD UNIQUE KEY `uq_oferente_puesto` (`id_oferente`,`id_puesto`),
  ADD KEY `idx_oferente_puesto_oferente` (`id_oferente`),
  ADD KEY `idx_oferente_puesto_puesto` (`id_puesto`);

--
-- Indices de la tabla `oferente_requisito`
--
ALTER TABLE `oferente_requisito`
  ADD PRIMARY KEY (`id_oferente`,`id_requisito`),
  ADD KEY `idx_oferente_requisito_requisito` (`id_requisito`);

--
-- Indices de la tabla `oferente_telf`
--
ALTER TABLE `oferente_telf`
  ADD PRIMARY KEY (`id_of_telefono`),
  ADD KEY `id_oferente` (`id_oferente`);

--
-- Indices de la tabla `pantallas`
--
ALTER TABLE `pantallas`
  ADD PRIMARY KEY (`id_pantalla`);

--
-- Indices de la tabla `parametros`
--
ALTER TABLE `parametros`
  ADD PRIMARY KEY (`id_parametro`);

--
-- Indices de la tabla `personas`
--
ALTER TABLE `personas`
  ADD PRIMARY KEY (`id_persona`),
  ADD UNIQUE KEY `identificacion` (`identificacion`);

--
-- Indices de la tabla `prepara_academica`
--
ALTER TABLE `prepara_academica`
  ADD PRIMARY KEY (`id_pre_academica`),
  ADD KEY `id_oferente` (`id_oferente`),
  ADD KEY `id_insti_edu` (`id_insti_edu`);

--
-- Indices de la tabla `provincias`
--
ALTER TABLE `provincias`
  ADD PRIMARY KEY (`id_provincia`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `puestos`
--
ALTER TABLE `puestos`
  ADD PRIMARY KEY (`id_puesto`),
  ADD KEY `id_puesto_jefac` (`id_puesto_jefac`);

--
-- Indices de la tabla `requisitos_puesto`
--
ALTER TABLE `requisitos_puesto`
  ADD PRIMARY KEY (`id_requisito`),
  ADD KEY `id_puesto` (`id_puesto`);

--
-- Indices de la tabla `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`id_rol`);

--
-- Indices de la tabla `rolpantalla`
--
ALTER TABLE `rolpantalla`
  ADD PRIMARY KEY (`id_rol`,`id_pantalla`),
  ADD KEY `FK_RolPantalla_Pantalla` (`id_pantalla`);

--
-- Indices de la tabla `usuariorol`
--
ALTER TABLE `usuariorol`
  ADD PRIMARY KEY (`id_usuario`,`id_rol`),
  ADD KEY `id_rol` (`id_rol`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_usuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `accion_personal`
--
ALTER TABLE `accion_personal`
  MODIFY `id_accion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT de la tabla `admin_area`
--
ALTER TABLE `admin_area`
  MODIFY `id_area` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `bitacoras`
--
ALTER TABLE `bitacoras`
  MODIFY `id_bitacoras` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `canton`
--
ALTER TABLE `canton`
  MODIFY `id_canton` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `companias`
--
ALTER TABLE `companias`
  MODIFY `id_compania` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `concursos`
--
ALTER TABLE `concursos`
  MODIFY `id_concursos` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `distrito`
--
ALTER TABLE `distrito`
  MODIFY `id_distrito` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `empleados`
--
ALTER TABLE `empleados`
  MODIFY `id_empleado` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT de la tabla `entrevistas`
--
ALTER TABLE `entrevistas`
  MODIFY `id_entrevista` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `experiencia_laboral`
--
ALTER TABLE `experiencia_laboral`
  MODIFY `id_experiencia` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `institu_educa`
--
ALTER TABLE `institu_educa`
  MODIFY `id_insti_edu` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `oferentes`
--
ALTER TABLE `oferentes`
  MODIFY `id_oferente` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `oferente_concur`
--
ALTER TABLE `oferente_concur`
  MODIFY `id_of_concurso` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `oferente_correo`
--
ALTER TABLE `oferente_correo`
  MODIFY `id_of_correo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `oferente_puesto`
--
ALTER TABLE `oferente_puesto`
  MODIFY `id_oferente_puesto` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `oferente_telf`
--
ALTER TABLE `oferente_telf`
  MODIFY `id_of_telefono` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `pantallas`
--
ALTER TABLE `pantallas`
  MODIFY `id_pantalla` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT de la tabla `parametros`
--
ALTER TABLE `parametros`
  MODIFY `id_parametro` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `personas`
--
ALTER TABLE `personas`
  MODIFY `id_persona` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `prepara_academica`
--
ALTER TABLE `prepara_academica`
  MODIFY `id_pre_academica` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `provincias`
--
ALTER TABLE `provincias`
  MODIFY `id_provincia` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `puestos`
--
ALTER TABLE `puestos`
  MODIFY `id_puesto` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `requisitos_puesto`
--
ALTER TABLE `requisitos_puesto`
  MODIFY `id_requisito` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `roles`
--
ALTER TABLE `roles`
  MODIFY `id_rol` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `accion_personal`
--
ALTER TABLE `accion_personal`
  ADD CONSTRAINT `accion_personal_ibfk_1` FOREIGN KEY (`id_empleado`) REFERENCES `empleados` (`id_empleado`),
  ADD CONSTRAINT `accion_personal_ibfk_2` FOREIGN KEY (`id_jefactura`) REFERENCES `empleados` (`id_empleado`);

--
-- Filtros para la tabla `bitacoras`
--
ALTER TABLE `bitacoras`
  ADD CONSTRAINT `bitacoras_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`),
  ADD CONSTRAINT `fk_bitacoras_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`);

--
-- Filtros para la tabla `canton`
--
ALTER TABLE `canton`
  ADD CONSTRAINT `canton_ibfk_1` FOREIGN KEY (`id_provincia`) REFERENCES `provincias` (`id_provincia`);

--
-- Filtros para la tabla `distrito`
--
ALTER TABLE `distrito`
  ADD CONSTRAINT `distrito_ibfk_1` FOREIGN KEY (`id_canton`) REFERENCES `canton` (`id_canton`);

--
-- Filtros para la tabla `empleados`
--
ALTER TABLE `empleados`
  ADD CONSTRAINT `empleados_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`),
  ADD CONSTRAINT `empleados_ibfk_2` FOREIGN KEY (`id_puesto`) REFERENCES `puestos` (`id_puesto`),
  ADD CONSTRAINT `empleados_ibfk_3` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`);

--
-- Filtros para la tabla `entrevistas`
--
ALTER TABLE `entrevistas`
  ADD CONSTRAINT `entrevistas_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`),
  ADD CONSTRAINT `entrevistas_ibfk_2` FOREIGN KEY (`id_empleado`) REFERENCES `empleados` (`id_empleado`);

--
-- Filtros para la tabla `experiencia_laboral`
--
ALTER TABLE `experiencia_laboral`
  ADD CONSTRAINT `experiencia_laboral_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`);

--
-- Filtros para la tabla `oferentes`
--
ALTER TABLE `oferentes`
  ADD CONSTRAINT `oferentes_ibfk_1` FOREIGN KEY (`id_persona`) REFERENCES `personas` (`id_persona`);

--
-- Filtros para la tabla `oferente_concur`
--
ALTER TABLE `oferente_concur`
  ADD CONSTRAINT `oferente_concur_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`),
  ADD CONSTRAINT `oferente_concur_ibfk_2` FOREIGN KEY (`id_concursos`) REFERENCES `concursos` (`id_concursos`);

--
-- Filtros para la tabla `oferente_correo`
--
ALTER TABLE `oferente_correo`
  ADD CONSTRAINT `oferente_correo_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`);

--
-- Filtros para la tabla `oferente_puesto`
--
ALTER TABLE `oferente_puesto`
  ADD CONSTRAINT `oferente_puesto_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`),
  ADD CONSTRAINT `oferente_puesto_ibfk_2` FOREIGN KEY (`id_puesto`) REFERENCES `puestos` (`id_puesto`);

--
-- Filtros para la tabla `oferente_requisito`
--
ALTER TABLE `oferente_requisito`
  ADD CONSTRAINT `fk_oferente_requisito_oferente` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_oferente_requisito_requisito` FOREIGN KEY (`id_requisito`) REFERENCES `requisitos_puesto` (`id_requisito`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `oferente_telf`
--
ALTER TABLE `oferente_telf`
  ADD CONSTRAINT `oferente_telf_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`);

--
-- Filtros para la tabla `prepara_academica`
--
ALTER TABLE `prepara_academica`
  ADD CONSTRAINT `prepara_academica_ibfk_1` FOREIGN KEY (`id_oferente`) REFERENCES `oferentes` (`id_oferente`),
  ADD CONSTRAINT `prepara_academica_ibfk_2` FOREIGN KEY (`id_insti_edu`) REFERENCES `institu_educa` (`id_insti_edu`);

--
-- Filtros para la tabla `puestos`
--
ALTER TABLE `puestos`
  ADD CONSTRAINT `puestos_ibfk_1` FOREIGN KEY (`id_puesto_jefac`) REFERENCES `puestos` (`id_puesto`);

--
-- Filtros para la tabla `requisitos_puesto`
--
ALTER TABLE `requisitos_puesto`
  ADD CONSTRAINT `requisitos_puesto_ibfk_1` FOREIGN KEY (`id_puesto`) REFERENCES `puestos` (`id_puesto`);

--
-- Filtros para la tabla `rolpantalla`
--
ALTER TABLE `rolpantalla`
  ADD CONSTRAINT `FK_RolPantalla_Pantalla` FOREIGN KEY (`id_pantalla`) REFERENCES `pantallas` (`id_pantalla`),
  ADD CONSTRAINT `FK_RolPantalla_Rol` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`);

--
-- Filtros para la tabla `usuariorol`
--
ALTER TABLE `usuariorol`
  ADD CONSTRAINT `usuariorol_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`),
  ADD CONSTRAINT `usuariorol_ibfk_2` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
