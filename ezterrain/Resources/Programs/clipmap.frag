#version 130

uniform sampler2D gradient;
uniform vec3 lightDirection;

in vec3 normal;
in float bias;

out vec4 fragColor;

void main()
{
	fragColor = dot(normal, lightDirection) * texture2D(gradient, vec2(bias, 0.0));
}
