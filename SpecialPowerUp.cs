using System.Collections.Generic;

public class SpecialPowerUp {
    public enum PowerUpRarity { NORMAL, RARE, EPIC }
    public string Name = "Nombre mejora";
    public string Description = "Descripción mejora";
    public PowerUpRarity Rarity = PowerUpRarity.NORMAL;
    public string Icon_Name = "Icon.png";
    public bool IsConsumable = false;
    public bool IsOneTime = false;

    private static List<SpecialPowerUp> _powerUps;

    public SpecialPowerUp(string name, string description, PowerUpRarity rarity, string icon_Name) {
        Name = name;
        Description = description;
        Rarity = rarity;
        Icon_Name = icon_Name;
    }

    public SpecialPowerUp(string name, string description, PowerUpRarity rarity, string icon_Name, bool isConsumable) {
        Name = name;
        Description = description;
        Rarity = rarity;
        Icon_Name = icon_Name;
        IsConsumable = isConsumable;
    }

    public SpecialPowerUp(bool oneTime, string name, string description, PowerUpRarity rarity, string icon_Name) {
        Name = name;
        Description = description;
        Rarity = rarity;
        Icon_Name = icon_Name;
        IsOneTime = oneTime;
    }

    static public List<SpecialPowerUp> AvailablePowerUps() {
        //NOTE: Always re-inits the array
        InitPowerUps();
        return _powerUps;
    }



    private static void InitPowerUps() {
        _powerUps = new List<SpecialPowerUp>();
        //REPETEABLES
        _powerUps.Add(new SpecialPowerUp("Mas resistencia", "Incrementa en 1 tus puntos de vida maximos", PowerUpRarity.NORMAL, "Powers_16"));
        _powerUps.Add(new SpecialPowerUp("Mas shurikens", "Incrementa en 1 tus shurikens maximos", PowerUpRarity.NORMAL, "Powers_17"));
        _powerUps.Add(new SpecialPowerUp("Afilar espada", "Incrementa en 1 el daño de espada", PowerUpRarity.NORMAL, "Powers_10"));
        //CONSUMABLES
        _powerUps.Add(new SpecialPowerUp("Restauración", "Recupera hasta 3 puntos de vida", PowerUpRarity.NORMAL, "Powers_9", true));
        //SINGLE USE
        _powerUps.Add(new SpecialPowerUp(true, "Shuriken de goma", "Los shurikens rebotan aleatoreamente una vez", PowerUpRarity.RARE, "Powers_11"));
        _powerUps.Add(new SpecialPowerUp(true, "Escudo temporal", "Obten un escudo luego de unos segundos sin recibir daño", PowerUpRarity.RARE, "Powers_1"));
        _powerUps.Add(new SpecialPowerUp(true, "Distorsión temporal", "Al quedar en critico (1 vida), realentiza el tiempo unos segundos", PowerUpRarity.RARE, "Powers_2"));
        _powerUps.Add(new SpecialPowerUp(true, "Robo de alma", "Probabilidad de curarse al matar un enemigo", PowerUpRarity.RARE, "Powers_3"));
        _powerUps.Add(new SpecialPowerUp(true, "Bomba de shuriken", "Al quedar en critico (1 vida), tiras shurikens normales en todas direcciones", PowerUpRarity.RARE, "Powers_4"));
        _powerUps.Add(new SpecialPowerUp(true, "Golpe cargado", "Manten boton para cargar y liberar una ola de energia", PowerUpRarity.RARE, "Powers_5"));
        //_powerUps.Add(new SpecialPowerUp(true, "Ola reflectante", "La ola de energia refleja los proyectiles enemigos", PowerUpRarity.RARE, "Powers_6"));
        _powerUps.Add(new SpecialPowerUp(true, "Shuriken explosivo", "Al impactar el suelo o paredes, tus shurikens explotan", PowerUpRarity.RARE, "Powers_7"));
        //_powerUps.Add(new SpecialPowerUp(true, "Shuriken fragmentado", "Al impactar el suelo o paredes, tus shurikens despliegan 3 esquirlas", PowerUpRarity.RARE, "Powers_8"));
        //_powerUps.Add(new SpecialPowerUp(true, "Dos estrellas", "Tiras 2 shurikens pequeños en un angulo, en vez de uno grande", PowerUpRarity.RARE, "Powers_12"));
        _powerUps.Add(new SpecialPowerUp(true, "Deslizado largo", "Tu deslizado alcanza mayor distancia", PowerUpRarity.RARE, "Powers_13"));
        _powerUps.Add(new SpecialPowerUp(true, "Deslizado seguro", "Obtienes un escudo por un breve periodo luego de deslizarte", PowerUpRarity.RARE, "Powers_14"));
        _powerUps.Add(new SpecialPowerUp(true, "Paso de sombra", "Al caer del escenario te proyectas a una posición aleatorea (1 vez por mapa)", PowerUpRarity.RARE, "Powers_15"));
        _powerUps.Add(new SpecialPowerUp(true, "Tres estrellas", "Tiras 3 shurikens pequeños en un angulo, en vez de uno grande", PowerUpRarity.RARE, "Powers_0"));
    }
}
