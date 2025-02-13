const fs = require("fs");
const { exec } = require("child_process");

// Get environment argument
const env = process.argv[2] || "development";
const envFile = `./src/environments/environment.${env}.ts`;

const environmentFile = fs.existsSync(envFile)
  ? envFile
  : "./src/environments/environment.ts";
console.log(`✅ Using environment file: ${environmentFile}`);

const environmentContent = fs.readFileSync(environmentFile, "utf8");
const swaggerUrlMatch = environmentContent.match(/swagger:\s*['"]([^'"]+)['"]/);

if (!swaggerUrlMatch) {
  console.error(
    "❌ Error: Could not find swagger url in the selected environment file"
  );
  process.exit(1);
}

const swaggerUrl = swaggerUrlMatch[1];
console.log(`✅ Found API URL: ${swaggerUrl}`);

const nswagCommand = `npx nswag openapi2tsclient /input:${swaggerUrl} /output:src/app/services/api-client.ts /template:angular`;

console.log(`🚀 Running NSwag command: ${nswagCommand}`);

exec(nswagCommand, (error, stdout, stderr) => {
  if (error) {
    console.error(`❌ NSwag generation failed: ${error.message}`);
    return;
  }
  if (stderr) {
    console.error(`⚠ Warning: ${stderr}`);
  }
  console.log(`✅ NSwag generation successful: ${stdout}`);
});
