# **JAM?: A Storytelling Game**

## **📌 Overview**
JAM? is a **narrative-driven** interactive game developed in **Unity** using **C#**. The game explores themes of **AI domination**, human creativity, and the struggle for identity in an increasingly automated world. Players take control of **Anita**, a woman whose life is shattered after her job is replaced by AI and her relationship is torn apart by synthetic emotions.

## **🎮 Features**
- **Interactive Storytelling**: The game allows players to make meaningful choices that shape Anita's journey and the world's fate.
- **Multiple Story Paths**: Choices unlock different narrative branches, each leading to unique consequences.
- **Custom Graphics**: All art assets were created in-house by our team.
- **Dynamic Audio System**: A smooth transition system between scene-specific and persistent background music.
- **Save & Load System**: Players can **save progress** and **load previous checkpoints**.

## **🛠️ Development Stack**
- **Engine**: Unity
- **Programming Language**: C#
- **Audio Implementation**: Unity Audio Mixer, adaptive background music transitions
- **Story Format**: JSON-based script for dialogue and scene progression
- **Graphics**: 2D assets designed by our team

## **📜 Gameplay Overview**
The game follows **Anita**, an artist navigating a world increasingly controlled by AI. Players will make critical decisions that define her path, from embracing AI to resisting it or seeking an entirely different solution. Every choice impacts the story and its outcome.

## **🛠️ How to Play**
1. **Launch the Game** in Unity or export a build.
2. **Make Choices** that affect the narrative.
3. **Explore AI's impact** on Anita’s life and the world.
4. **Save and Load Progress** to experience different paths.
5. **Unravel the AI Conspiracy** or find a way to coexist.

## **🎵 Audio & Music**
- Dynamic transitions between **scene-specific and persistent background tracks**.
- **Custom-composed soundtrack** enhances immersion.

## **📁 File Structure**
- **/Assets/** → Unity project files.
- **/Scripts/** → C# scripts for game logic, dialogue system, and audio.
- **/Assets/Resources/** → Custom-made visual assets.
- **/Assets/Resources/Music/** → Sound effects and background music.
- **script.json** → The structured narrative flow and dialogue system.

## **🛠️ Installation & Running the Game**
1. **Open Unity** and load the project folder.
2. Click **Play** in the Unity Editor.
3. To build an executable version:
   - Go to **File > Build Settings**.
   - Select **platform (Windows, macOS, etc.)**.
   - Click **Build & Run**.

## **👥 Development Team**
- **Story & Narrative Design**: Timur Cravțov & Vladimir Vitcovschii & Artur Țugui
- **Programming (C# & Unity)**: Timur Cravțov & Vladimir Vitcovschii & Nicolae Marga & Alexandru Rudoi
- **Art & Graphics**: Timur Cravțov & Vladimir Vitcovschii & Nicolae Marga
- **Sound Design & Music**: Alexandru Rudoi 
- **QA & Playtesting**: Timur Cravțov & Vladimir Vitcovschii & Nicolae Marga & Alexandru Rudoi & Artur Țugui

## **📝 License & Credits**
- All assets and code were created **in-house**.
- This project is **for educational purposes** under the Tehnologii Multimedia course.
- External tools used: Unity, FL Studio.

---
Thank you for checking out JAM?! We hope you enjoy the experience and the philosophical dilemmas it presents. 🚀

Mermaid diagram of the plot:

```mermaid
graph TD
    A[start] --> B[1_starting_apartment]
    B -->|Choice: I make real art| C[1_a_response]
    B -->|Choice: I'll fight| D[1_b_response]
    C --> E[1_converge]
    D --> E
    E -->|Choice: Plug it in| F[1_a_night]
    E -->|Choice: Charge elsewhere| G[1_b_night]
    F --> H[1_night_converge]
    G --> H
    H --> I[2_infidelity_1]
    I --> J[3_infidelity_2]
    J -->|Choice: Get out| K[3_a_confront]
    J -->|Choice: How could you?| L[3_b_confront]
    K --> M[3_converge]
    L --> M
    M --> N[4_discarded]
    N -->|Choice: Unfair!| O[4_a_reaction]
    N -->|Choice: Should’ve seen it| P[4_b_reaction]
    O --> Q[4_converge]
    P --> Q
    Q --> R[5_the_last_job]
    R -->|Choice: Cog in machine| S[5_a_reflection]
    R -->|Choice: Can’t take spirit| T[5_b_reflection]
    S --> U[5_converge]
    T --> U
    U -->|Choice: Go home| R
    U -->|Choice: Talk to IT| V[6_talk_with_it_specialist]
    V -->|Choice: Terrifying| W[6_a_reaction]
    V -->|Choice: Stop it| X[6_b_reaction]
    V -->|Choice: Government weapon?| Y[6_c_question_it_specialist]
    W --> Z[6_converge]
    X --> Z
    Z -->|Choice: What glitches?| AA[6_b_0_ai_misinformation]
    Z -->|Choice: Don’t want to know| AB[6_a_the_suppressed_ending]
    AA -->|Choice: How can they?| AC[6_b_0_a_reaction]
    AA -->|Choice: No fixing?| AD[6_b_0_b_reaction]
    AC --> AE[6_b_0_converge]
    AD --> AE
    AE -->|Choice: Set it up| AF[6_b_5_meeting]
    AE -->|Choice: Think about it| AG[6_b_1_the_suppressed_ending]
    AF -->|Choice: Tell me more| AH[6_b_6_plan]
    AF -->|Choice: Not ready| AG
    AH -->|Choice: I’ll do it| AI[6_b_7_infiltration]
    AH -->|Choice: Can’t risk it| AG
    AI --> AJ[6_b_8_core]
    AJ --> AK[6_b_9_aftermath]
    Y --> AL[6_c_0_meet_old_hacker]
    AL -->|Choice: Use the key| AM[6_c_0_1_access_jam_files]
    AL -->|Choice: Too dangerous| AN[6_c_0_2_arrested_ending]
    AM --> AO[6_c_3_military_ending]
    
    AB:::ending
    AG:::ending
    AK:::ending
    AN:::ending
    AO:::ending

    classDef ending fill:#f9f,stroke:#333,stroke-width:2px;
```