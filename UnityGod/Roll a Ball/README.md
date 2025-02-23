# 🎮 Roll a Ball - Survival Edition

## 📌 Descripción
Este es un juego desarrollado en Unity basado en el clásico "Roll a Ball". En esta versión, además de recoger los pick-ups, el jugador debe evitar a un enemigo que lo persigue. El juego cuenta con dos niveles terminados y un tercer nivel que aún está en desarrollo.

## 🔥 Características del Juego
- **🔄 Movimiento del Jugador:** El jugador puede moverse usando el teclado o el sensor de movimiento del dispositivo.
- **🎥 Cambio de Cámara:** Se puede alternar entre vista en primera y tercera persona con la tecla `F`.
- **💀 Enemigo Perseguidor:** Un enemigo usa `NavMesh` para seguir al jugador y puede causar un `Game Over` si lo atrapa.
- **🌟 Pick-ups:** Al recogerlos, el jugador se vuelve temporalmente invulnerable y avanza en el juego.
- **✨ Teletransporte:** Al alcanzar ciertos puntos, el jugador puede ser transportado a nuevas áreas del nivel.
- **🌟 Estados del Jugador:** Implementación de diferentes estados (`Idle`, `Moving`, `Jumping`, `Invulnerable`) con animaciones.
- **♻️ Reinicio Automático:** Si el jugador pierde, el nivel se reinicia automáticamente tras unos segundos.

## 🎮 Controles
- **🛠️ Movimiento:** Teclas de dirección / Acelerómetro en dispositivos móviles.
- **🎥 Cambio de cámara:** `F`
- **⬆️ Saltar:** (Desactivado por ahora, pero se puede habilitar en el código.)

## 🖥️ Scripts Principales
### 🎥 `CameraController.cs`
Este script gestiona la cámara del juego y permite cambiar entre vista en primera y tercera persona. 
- **Seguimiento del jugador:** Usa `Transform` para mantener la cámara alineada con la posición del jugador.
- **Cambio de perspectiva:** Al presionar `F`, se ajusta la posición y rotación de la cámara para cambiar entre las vistas.
- **Suavizado del movimiento:** Implementa `Vector3.Lerp` para una transición fluida entre posiciones.

### 👿 `EnemyAI.cs`
Controla el comportamiento del enemigo, que persigue al jugador usando `NavMeshAgent`.
- **Movimiento automático:** Configura la posición de destino del `NavMeshAgent` hacia el jugador en `Update()`.
- **Colisión con el jugador:** Si el enemigo toca al jugador, se verifica si este es invulnerable.
  - Si el jugador no es invulnerable, se llama a `GameOver()`.
  - Si el jugador es invulnerable, se ignora la colisión.
- **Uso de `NavMeshAgent`:** Permite que el enemigo navegue de manera inteligente evitando obstáculos.

### 🏋️ `PlayerController.cs`
Este es el script más complejo, ya que gestiona múltiples aspectos del jugador:
- **Movimiento:** Usa `InputSystem` para detectar el input del jugador y aplicarlo mediante fuerzas (`Rigidbody`) o transformaciones directas en móviles (acelerómetro).
- **Estados del jugador:** Se manejan cuatro estados (`Idle`, `Moving`, `Jumping`, `Invulnerable`), los cuales afectan la animación y el comportamiento del personaje.
- **Recolección de pick-ups:** Al recoger un objeto, el jugador obtiene invulnerabilidad temporal y puede desbloquear nuevas áreas.
- **Teletransporte:** Se implementa con `Transform.position`, reiniciando la puntuación y activando nuevos enemigos.
- **Gestión de colisiones:** El script detecta colisiones con `OnTriggerEnter` y maneja eventos como `Game Over` o activación de teletransportes.
- **Animaciones:** Se usa un `Animator` para cambiar entre los diferentes estados visuales del jugador.
- **Reinicio del nivel:** Si el jugador pierde, el nivel se recarga automáticamente tras un tiempo con `SceneManager`.

### 🛠️ `Rotator.cs`
Aplica rotación a los pick-ups para darles un efecto visual llamativo.

## 🏆 Niveles
1. **Nivel 1:** Introducción al juego, aprendizaje del movimiento y primeros pick-ups.
2. **Nivel 2:** Aumenta la dificultad con un nuevo enemigo y teletransporte.
3. **Nivel 3 (En desarrollo):** Próximamente con nuevos desafíos y mecánicas.




