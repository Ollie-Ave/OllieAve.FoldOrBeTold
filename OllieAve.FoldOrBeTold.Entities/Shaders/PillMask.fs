#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;   // pill mask
uniform vec4 colDiffuse;
uniform float health;         // 0..1

out vec4 finalColor;

void main()
{
    vec4 mask = texture(texture0, fragTexCoord);

    // discard anything outside pill shape
    if (mask.a < 0.5) discard;

    // fill based on X position (simple left-to-right bar)
    if (fragTexCoord.x > health)
        discard;

    finalColor = colDiffuse;
}
