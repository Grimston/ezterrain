uniform float time;
uniform int color;
varying float bias;

void main()
{
	gl_FragColor = bias * vec4(1.0, 0.0, 0.0, 1.0);
}
