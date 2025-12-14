# CSE MCP Server

**Model Context Protocol Server for Colombo Stock Exchange**

A Model Context Protocol (MCP) server that provides structured, real-time and historical data access for the Colombo Stock Exchange (CSE), enabling AI agents and applications to query, analyze, and integrate Sri Lankan market data seamlessly.

## Overview

CSE MCP Server bridges the gap between the Colombo Stock Exchange and AI-powered applications by exposing CSE market data through the Model Context Protocol. This enables Claude AI and other MCP-compatible agents to access real-time quotes, company profiles, trading data, financial statements, and historical prices for all CSE-listed securities.

## Key Features

### Real-Time Market Data
- **Stock Quotes**: Current prices, changes, volumes, and market capitalization
- **Company Profiles**: Detailed company information, sector classification, and contact details
- **Trading Data**: Intraday trades, volumes, and price movements
- **Market Overview**: Indices, top gainers/losers, most active stocks

### Historical Data
- **Price History**: OHLCV data with flexible time periods and intervals
- **Financial Statements**: Quarterly and annual reports (when available)
- **Corporate Actions**: Dividends, splits, and announcements

### CSE-Specific Features
- **Multi-Stock Support**: Handle companies with multiple share types (voting/non-voting)
- **Symbol Flexibility**: Supports multiple symbol formats (JKH, JKH.CM, JKH.N0000)
- **Sri Lankan Market Context**: Sector classifications, trading hours, and market conventions

## Architecture

Built on Clean Architecture principles:
- **Domain Layer**: Core CSE entities and business rules
- **Application Layer**: Use cases and data transformation
- **Infrastructure Layer**: CSE API integration and data persistence
- **Presentation Layer**: MCP tools and protocol implementation

## Technology Stack

- **.NET 10**: Modern, high-performance runtime
- **MCP SDK**: Official .NET implementation of Model Context Protocol
- **CSE Official API**: Direct integration with Colombo Stock Exchange
- **HTTP/SSE Transport**: Server-Sent Events for real-time updates

## Quick Start

### Prerequisites
- .NET 10 SDK
- Claude Desktop (or any MCP-compatible client)

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd SharesMCP
```

2. Build the project:
```bash
dotnet build
```

3. Run the server:
```bash
dotnet run --project SharesMCP
```

The server will start on `http://localhost:5248` (default).

### Configure Claude Desktop

Add to your Claude Desktop configuration (`~/Library/Application Support/Claude/claude_desktop_config.json` on macOS):

```json
{
  "mcpServers": {
    "cse": {
      "url": "http://localhost:5248/sse"
    }
  }
}
```

Restart Claude Desktop to connect to the CSE MCP Server.

## Available MCP Tools

### Market Data Tools

#### `GetStockQuote`
Get current stock quote for any CSE-listed security.

**Parameters:**
- `symbol` (string): Stock symbol (e.g., "JKH", "JKH.CM", or "JKH.N0000")

**Example:**
```
User: What's the current price of John Keells Holdings?
Claude: [Uses GetStockQuote with symbol "JKH"]
```

#### `GetCompanyProfile`
Get detailed company information and profile.

**Parameters:**
- `symbol` (string): Stock symbol

#### `GetDayTrades`
Get today's trading activity and individual trades.

**Parameters:**
- `symbol` (string): Stock symbol

## CSE Stock Symbols

The server supports multiple symbol formats for all CSE-listed securities:

- **Short form**: `JKH`, `COMB`, `SAMP`
- **.CM format**: `JKH.CM`, `COMB.CM`, `SAMP.CM`
- **CSE format**: `JKH.N0000`, `COMB.N0000`, `SAMP.N0000`

### Popular CSE Stocks

- **JKH** - John Keells Holdings PLC
- **COMB** - Commercial Bank of Ceylon PLC
- **SAMP** - Sampath Bank PLC
- **DIAL** - Dialog Axiata PLC
- **HNB** - Hatton National Bank PLC
- **NDB** - NDB Bank PLC

## Use Cases

### For AI Agents
- Query real-time CSE market data
- Analyze Sri Lankan company fundamentals
- Track portfolio performance
- Generate investment insights

### For Developers
- Build CSE-aware applications
- Integrate Sri Lankan market data
- Create trading algorithms
- Develop financial dashboards

### For Investors
- Access CSE data through Claude AI
- Get instant company information
- Track real-time price movements
- Analyze market trends

## Roadmap

### Phase 0: Walking Skeleton ✅
- Basic MCP server with three tools
- CSE Official API integration
- Symbol format conversion
- Real-time data access

### Phase 1: Foundation (In Progress)
- Clean Architecture refactoring
- Domain model for CSE entities
- Enhanced caching layer
- Historical price data

### Phase 2: Extended Data Access
- Financial statements integration
- Corporate announcements
- Market indices and sectors
- Advanced filtering and search

### Phase 3: Analysis Capabilities
- Technical indicators
- Fundamental metrics
- Comparative analysis
- Market screening tools

### Phase 4: Production Features
- Rate limiting and resilience
- Comprehensive error handling
- Performance optimization
- Monitoring and logging

## Contributing

Contributions are welcome! This project aims to provide comprehensive CSE data access to the MCP ecosystem.

## Data Source

All market data is sourced from:
- **CSE Official API**: https://www.cse.lk/api/
- **CSE Website**: https://www.cse.lk

## License

[To be determined]

## Disclaimer

This MCP server provides market data access only. It does not provide investment advice or recommendations. Always conduct your own research and consult with qualified financial advisors before making investment decisions.

---

**Built for the Model Context Protocol ecosystem**

Enabling AI agents to access Colombo Stock Exchange data seamlessly.