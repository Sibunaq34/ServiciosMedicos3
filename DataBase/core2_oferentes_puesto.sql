DROP PROCEDURE IF EXISTS `sp_ObtenerOferentesPorPuesto`;

DELIMITER $$

CREATE PROCEDURE `sp_ObtenerOferentesPorPuesto`(
    IN `pCodigoPuesto` VARCHAR(20),
    IN `pPage` INT,
    IN `pPageSize` INT
)
BEGIN
    DECLARE `vIdPuesto` INT DEFAULT NULL;
    DECLARE `vOffset` INT DEFAULT 0;

    SET `vOffset` = (`pPage` - 1) * `pPageSize`;

    SELECT `id_puesto`
      INTO `vIdPuesto`
      FROM `puestos`
     WHERE `codigo_puesto` = TRIM(`pCodigoPuesto`)
       AND `activo` = 1
     LIMIT 1;

    IF `vIdPuesto` IS NULL THEN
        SELECT 0 AS `Total`;

        SELECT
            CAST(NULL AS SIGNED) AS `IdOferente`,
            CAST(NULL AS CHAR(150)) AS `NombreCompleto`,
            CAST(NULL AS CHAR(30)) AS `Identificacion`
        WHERE 1 = 0;
    ELSE
        SELECT COUNT(DISTINCT `op`.`id_oferente`) AS `Total`
        FROM `oferente_puesto` AS `op`
        INNER JOIN `oferentes` AS `o`
            ON `o`.`id_oferente` = `op`.`id_oferente`
        INNER JOIN `personas` AS `p`
            ON `p`.`id_persona` = `o`.`id_persona`
        WHERE `op`.`id_puesto` = `vIdPuesto`
          AND `op`.`estado` = 'Postulado';

        SELECT DISTINCT
            `o`.`id_oferente` AS `IdOferente`,
            `p`.`nombre_comple` AS `NombreCompleto`,
            `p`.`identificacion` AS `Identificacion`
        FROM `oferente_puesto` AS `op`
        INNER JOIN `oferentes` AS `o`
            ON `o`.`id_oferente` = `op`.`id_oferente`
        INNER JOIN `personas` AS `p`
            ON `p`.`id_persona` = `o`.`id_persona`
        WHERE `op`.`id_puesto` = `vIdPuesto`
          AND `op`.`estado` = 'Postulado'
        ORDER BY `p`.`nombre_comple` ASC, `o`.`id_oferente` ASC
        LIMIT `pPageSize` OFFSET `vOffset`;
    END IF;
END$$

DELIMITER ;
