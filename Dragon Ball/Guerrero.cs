using System;

namespace DragonBallTorneo
{
    // Requerimiento 1: Modelado inicial del dominio
    public class Guerrero
    {
        public double PotencialOfensivo { get; set; }
        public int Experiencia { get; set; }
        public double Energia { get; private set; }
        public double EnergiaMaxima { get; private set; }
        
        // Requerimiento 2: Incorporación del traje
        public Traje TrajeEquipado { get; set; }

        public Guerrero(double potencialOfensivo, double energiaMaxima, Traje trajeInicial = null)
        {
            PotencialOfensivo = potencialOfensivo;
            EnergiaMaxima = energiaMaxima;
            Energia = energiaMaxima; // Inicia con la energía al máximo
            Experiencia = 0;
            TrajeEquipado = trajeInicial;
        }

        public void Atacar(Guerrero otro)
        {
            // Cuando un guerrero ataca al otro, lo hace con todo su potencial ofensivo.
            double dañoBase = PotencialOfensivo * 0.10; 
            otro.RecibirAtaque(dañoBase);
        }

        public void RecibirAtaque(double dañoBase)
        {
            double dañoFinal = dañoBase;
            int experienciaGanada = 1;

            if (TrajeEquipado != null)
            {
                // Requerimiento 3: El traje se usa al ser atacado
                TrajeEquipado.Usar();

                // Si no está gastado, aplica sus efectos
                if (!TrajeEquipado.EstaGastado())
                {
                    dañoFinal = TrajeEquipado.CalcularDaño(dañoBase);
                    experienciaGanada = TrajeEquipado.CalcularExperiencia(experienciaGanada);
                }
            }

            Energia -= dañoFinal;
            if (Energia < 0) Energia = 0; // La energía llega a cero cuando mueren.

            Experiencia += experienciaGanada;
        }

        public void ComerSemillaErmitaño()
        {
            Energia = EnergiaMaxima; 
        }

        public bool EstaMuerto() => Energia <= 0;
    }

    // Requerimiento 2 y 3: Clases base para los trajes y sistema de desgaste
    public abstract class Traje
    {
        // Todos los trajes nuevos tienen un nivel de desgaste, que comienza en cero.
        public int Desgaste { get; protected set; } = 0;

        public bool EstaGastado()
        {
            return Desgaste >= 100;
        }

        public void Usar()
        {
            if (!EstaGastado())
            {
                Desgaste += 5;
            }
        }

        // Métodos abstractos/virtuales para aplicar polimorfismo
        public abstract double CalcularDaño(double dañoBase);
        
        public virtual int CalcularExperiencia(int experienciaBase)
        {
            return experienciaBase; // Comportamiento por defecto
        }
    }

    public class TrajeComun : Traje
    {
        public double PorcentajeProteccion { get; set; }

        public TrajeComun(double porcentajeProteccion)
        {
            PorcentajeProteccion = porcentajeProteccion;
        }

        public override double CalcularDaño(double dañoBase)
        {
            return dañoBase - (dañoBase * (PorcentajeProteccion / 100.0));
        }
    }

    public class TrajeEntrenamiento : Traje
    {
        // Propiedad estática: es el mismo para todos los trajes de entrenamiento y puede cambiar.
        public static double MultiplicadorExperiencia { get; set; } = 2.0;

        public override double CalcularDaño(double dañoBase)
        {
            return dañoBase; // No disminuyen el daño recibido
        }

        public override int CalcularExperiencia(int experienciaBase)
        {
            return (int)(experienciaBase * MultiplicadorExperiencia);
        }
    }
}