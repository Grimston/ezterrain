uniform sampler2D noise;
uniform vec3 lightDirection;

varying vec3 normal;

//varying float red;
//varying float blue;

void main()
{
	gl_FragColor = dot(normal, lightDirection) * texture2D(noise, gl_TexCoord[0].st);// * vec4(red, 1.0, blue, 1.0);
}
