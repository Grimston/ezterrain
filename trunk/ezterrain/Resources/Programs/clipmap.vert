uniform sampler2D noise;

uniform float texScale;
uniform float texOffset;
uniform float heightScale;

uniform float level;

uniform vec3 eye;

varying vec3 normal;
varying float bias;

//varying float red;
//varying float blue;

vec4 calcTexCoord(vec4 vertex)
{
	return (vertex + texOffset) * texScale;
}

float getBias(vec4 texCoord)
{
	return tex2D(noise, texCoord.st).r;
}

float getHeight(float bias)
{
	return heightScale * bias;
}

float getHeight(vec4 texCoord)
{
	return getHeight(getBias(texCoord));
}

void main()
{
	//red = mod(gl_Vertex.x, 2.0);
	//blue = mod(gl_Vertex.y, 2.0);
	
	float vertexScale = pow(2.0, level);
	
	vec4 vertex = (gl_Vertex  * vertexScale) + vec4(eye.x, eye.y, 0.0, 0.0);

	gl_TexCoord[0] = calcTexCoord(vertex);
	bias = getBias(gl_TexCoord[0]);
	vertex.z = getHeight(bias);
	vertex.w = 1.0;
	
	vec4 komsu1 = vertex;
	//TODO: check bounds
	komsu1.x = komsu1.x + vertexScale;
	komsu1.z = getHeight(calcTexCoord(komsu1));
	vec3 delta1 = normalize((komsu1 - vertex).xyz);
	
	vec4 komsu2 = vertex;
	//TODO: check bounds
	komsu2.y = komsu2.y + vertexScale;
	komsu2.z = getHeight(calcTexCoord(komsu2));
	vec3 delta2 = normalize((komsu2 - vertex).xyz);
	
	vec4 komsu3 = vertex;
	//TODO: check bounds
	komsu3.x = komsu3.x - vertexScale;
	komsu3.z = getHeight(calcTexCoord(komsu3));
	vec3 delta3 = normalize((komsu3 - vertex).xyz);
	
	vec4 komsu4 = vertex;
	//TODO: check bounds
	komsu4.y = komsu4.y - vertexScale;
	komsu4.z = getHeight(calcTexCoord(komsu4));
	vec3 delta4 = normalize((komsu4 - vertex).xyz);

	vec3 normal1 = cross(delta1, delta2);
	vec3 normal2 = cross(delta2, delta3);
	vec3 normal3 = cross(delta3, delta4);
	vec3 normal4 = cross(delta4, delta1);
	normal = (normal1 + normal2 + normal3 + normal4) / 4.0;
	
	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
