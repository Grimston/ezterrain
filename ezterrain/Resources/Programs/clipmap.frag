uniform sampler2D gradient;
uniform vec3 lightDirection;

varying vec3 normal;
varying float bias;

//varying float red;
//varying float blue;

void main()
{
	gl_FragColor = dot(normal, lightDirection) * texture2D(gradient, vec2(bias, 0.0));// * vec4(red, 1.0, blue, 1.0);
}
