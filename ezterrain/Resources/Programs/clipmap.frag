uniform sampler2D noise;

varying vec3 normal;

void main()
{
	gl_FragColor = dot(normal, vec3(0, 0, 1)) * texture2D(noise, gl_TexCoord[0].st);
}
